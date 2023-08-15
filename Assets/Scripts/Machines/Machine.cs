using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Elympics;
using TMPro;
using System;
using Unity.VisualScripting;

public class Machine : ElympicsMonoBehaviour, IObservable
{
    [SerializeField] private Canvas TasksUI;
    [SerializeField] private GameObject TasksPanel;
    [SerializeField] private GameObject Broken;
    [SerializeField] private GameObject _TaskPrefab;

    public List<TaskData> machineTasks = new List<TaskData>();
    public List<SabotageData> machineSabotage = new List<SabotageData>();
    private MachineData machineData;
    public ElympicsBool isBroken = new ElympicsBool(false);
    public ElympicsBool prankBroken = new ElympicsBool(false);

    private ElympicsInt taskIndex = new ElympicsInt(-1);
    private ElympicsFloat progress = new ElympicsFloat(0);

    Slider slider;
    private Transform currentPlayer;

    void Start()
    {
        SetMachineData();
        LoadTaskUI();
        TasksUI.gameObject.SetActive(false);
        if(Elympics.IsServer)
        prankBroken.ValueChanged += PrankSet;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(currentPlayer == null && col.transform.tag == "Work") 
        {
            currentPlayer = col.transform;
            currentPlayer.Find("Canvas").Find("Missing").gameObject.SetActive(false);
            slider = currentPlayer.Find("Canvas").Find("Slider").GetComponent<Slider>();

            //turn on UI only for interacting player
            var player = currentPlayer.parent.GetComponent<ElympicsBehaviour>();
            if(Elympics.Player != player.PredictableFor) return;
            TasksUI.gameObject.SetActive(true);
            TasksPanel.SetActive(true);
            if (isBroken.Value) 
            {
                Broken.SetActive(true);
                TasksPanel.SetActive(false);
            }
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        //dont let other players to interact now
        if (isBroken.Value) 
        {
            slider.gameObject.SetActive(false);
            TasksUI.gameObject.SetActive(true);
            TasksPanel.SetActive(false);
            Broken.SetActive(true);
            return;
        }
        if (col.transform == currentPlayer) 
        {
            //check if task chosen
            var taskID = currentPlayer.parent.GetComponent<PlayerHandler>().taskID;
            if (taskID != -1)
            {
                taskIndex.Value = taskID;
                currentPlayer.parent.GetComponent<PlayerHandler>().taskID = -1;
                //if prank is set abort task execution and set restart timer
                if(prankBroken.Value)
                {
                    prankBroken.Value = false;
                    isBroken.Value = true;
                    return;
                }
                //if task is sabotaging
                if (taskID < 100)
                {
                    slider.maxValue = machineTasks.Find(x => x.ID == taskIndex.Value).TaskTime;
                    Debug.Log("Taskindex: " + taskIndex.Value);
                }
                //if normal task
                else
                {
                    slider.maxValue = machineSabotage.Find(x => x.ID == taskIndex.Value).TaskTime;
                    Debug.Log("Taskindex: " + taskIndex.Value);
                }
            }
            //if task chosen, let work
            if(taskIndex.Value != -1 && HasItems())
            {
                if (Elympics.Player == PredictableFor)
                    slider.gameObject.SetActive(true);
                if(col.transform.parent.GetComponent<Actions>().IsWorking()) 
                {
                    progress.Value += Time.deltaTime;
                }
                else 
                {
                    progress.Value = 0;
                }
                //Debug.Log(progress);
                slider.value = progress.Value;
                if (progress.Value > slider.maxValue)
                {
                    Debug.Log("Task Completed");
                    TaskCompleted();
                    RemoveItems();
                    if (taskIndex.Value > 99 && Elympics.IsServer)
                    {
                        isBroken.Value = true;
                        machineData.timeDown.Value -= machineSabotage.Find(x => x.ID == taskIndex.Value).TaskTime;
                        StartCoroutine("TimeDown");
                    }
                    taskIndex.Value = -1;
                }
            }
            else 
            {
                slider.gameObject.SetActive(false);
                if (taskIndex.Value != -1 && !HasItems())
                {
                    currentPlayer.Find("Canvas").Find("Missing").gameObject.SetActive(true);
                }
            }

        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        // if (col.transform.tag == "Work") {
        if (currentPlayer != null && col.transform == currentPlayer) 
        {
            currentPlayer = null;
            taskIndex.Value = -1;
            progress.Value = 0;
            slider.value = 0;
            col.transform.Find("Canvas").Find("Missing").gameObject.SetActive(false);
            slider.gameObject.SetActive(false);
            Broken.SetActive(false);
            TasksUI.gameObject.SetActive(false);
        }
    }

    private void PrankSet(bool oldv, bool newv)
    {
        if (newv)
        {
            StopCoroutine("TimeDown");
            isBroken.Value = false;
        }
        else
        {
            StartCoroutine("TimeDown");
        }
    }

    private IEnumerator TimeDown()
    {
        Debug.Log("machine time: " + machineData.timeDown.Value);
        yield return new WaitForSeconds(machineData.timeDown.Value);
        Debug.Log("unlocking");
        isBroken.Value = false;
    }

    private bool HasItems()
    {
        if(taskIndex.Value > 99)
        {
            var requirements = machineSabotage.Find(x => x.ID == taskIndex.Value).requirements;
            foreach(ItemData item in requirements)
            {
                if (!currentPlayer.GetComponentInParent<InventoryManager>().HasItem(item.ID)) return false;
            }
            return true;
        }
        else
        {
            var requirements = machineTasks.Find(x => x.ID == taskIndex.Value).requirements;
            foreach(ItemData item in requirements)
            {
                if (!currentPlayer.GetComponentInParent<InventoryManager>().HasItem(item.ID)) return false;
            }
            return true;
        }

    }

    private void RemoveItems()
    {
        if (taskIndex.Value > 99)
        {
            var requirements = machineSabotage.Find(x => x.ID == taskIndex.Value).requirements;
            foreach(ItemData item in requirements)
            {
                currentPlayer.GetComponentInParent<InventoryManager>().RemoveItem(item.ID);
            }
        }
        else
        {
            var requirements = machineTasks.Find(x => x.ID == taskIndex.Value).requirements;
            foreach(ItemData item in requirements)
            {
                currentPlayer.GetComponentInParent<InventoryManager>().RemoveItem(item.ID);
            }
        }

    }

    private void TaskCompleted()
    {
        currentPlayer.parent.GetComponent<TaskManager>().OnTaskCompleted(taskIndex.Value);
    }


    private void SetMachineData()
    {
        machineData = ScriptableObject.CreateInstance<MachineData>();
        machineData.timeDown.Value = 5 * machineTasks.Count;
    }

    #region Machine Tasks UI
    public void LoadTaskUI() 
    {
        for(int i=0; i<machineTasks.Count; i++)
        {
            var taskui = Instantiate(_TaskPrefab);
            taskui.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>().text = machineTasks[i].Description;
            taskui.name = machineTasks[i].ID.ToString();
            taskui.transform.SetParent(TasksPanel.transform);
            taskui.GetComponent<Button>().onClick.AddListener(delegate() 
            {
                int.TryParse(taskui.name, out int index);
                TaskClicked(index);
            });
        }
        for(int i=0; i<machineSabotage.Count; i++)
        {
            var taskui = Instantiate(_TaskPrefab);
            taskui.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>().text = machineSabotage[i].Description;
            taskui.name = machineSabotage[i].ID.ToString();
            taskui.transform.SetParent(TasksPanel.transform);
            taskui.GetComponent<Image>().color = Color.red;
            taskui.GetComponent<Button>().onClick.AddListener(delegate() 
            {
                int.TryParse(taskui.name, out int index);
                Debug.Log("clicked: " + index);
                TaskClicked(index);
            });
        }
    }

    public void TaskClicked(int index)
    {
        currentPlayer.parent.GetComponent<Inputs>().inputStruct.taskID = index;
        //taskIndex.Value = index;
        //slider.maxValue = machineTasks[index].TaskTime;
        TasksUI.gameObject.SetActive(false);
    }
    #endregion
}
