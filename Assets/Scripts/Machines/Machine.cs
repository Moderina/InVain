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
    [SerializeField] private MachineLook machineLook;
    [SerializeField] private Canvas MachineUI;
    [SerializeField] private GameObject TasksPanel;
    [SerializeField] private SliderSliding sliderSliding;
    [SerializeField] private GameObject Broken;
    [SerializeField] private GameObject _TaskPrefab;

    public List<TaskData> machineTasks = new List<TaskData>();
    public List<SabotageData> machineSabotage = new List<SabotageData>();
    private MachineData machineData;
    public ElympicsBool isBroken = new ElympicsBool(false);
    public ElympicsBool prankBroken = new ElympicsBool(false);

    private ElympicsInt taskIndex = new ElympicsInt(-1);
    private ElympicsInt ammount = new ElympicsInt(0);
    private ElympicsFloat progress = new ElympicsFloat(0);

    Slider slider;
    private Transform currentPlayer;

    void Start()
    {
        SetMachineData();
        MachineUI.gameObject.SetActive(false);
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
            TasksPanel.SetActive(true);
            if (isBroken.Value) 
            {
                MachineUI.gameObject.SetActive(true);
                Broken.SetActive(true);
                TasksPanel.SetActive(false);
            }
            LoadTaskUI();
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
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
                    OnTriggerExit2D(col);
                    OnTriggerEnter2D(col);
                    return;
                }
                //if task is sabotaging
                if (taskID < 100)
                {
                    // slider.maxValue = machineTasks.Find(x => x.ID == taskIndex.Value).TaskTime;
                    sliderSliding.SetTask(machineTasks.Find(x => x.ID == taskIndex.Value).width);
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
                // var player = currentPlayer.parent.GetComponent<ElympicsBehaviour>();
                // if(Elympics.Player == player.PredictableFor)
                //     slider.gameObject.SetActive(true);
                if(currentPlayer.parent.GetComponent<Actions>().IsWorking()) 
                {
                    // progress.Value += Time.deltaTime;
                    if(sliderSliding.IsInside())
                    {
                        ammount.Value += 1;
                        Debug.Log("We got the moves");
                    }
                }
                // else 
                // {
                //     progress.Value = 0;
                // }
                //Debug.Log(progress);
                slider.value = progress.Value;
                // if (progress.Value > slider.maxValue)
                if(ammount.Value == machineTasks.Find(x => x.ID == taskIndex.Value).ammount)
                {
                    Debug.Log("Task Completed");
                    TaskCompleted();
                    RemoveItems();
                    MachineUI.gameObject.SetActive(false);
                    if (taskIndex.Value > 99 && Elympics.IsServer)
                    {
                        isBroken.Value = true;
                        machineData.timeDown.Value -= machineSabotage.Find(x => x.ID == taskIndex.Value).TaskTime;
                        StartCoroutine("TimeDown");
                    }
                    taskIndex.Value = -1;
                    return;
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
            CleanUI();
            TasksPanel.gameObject.SetActive(false);
            MachineUI.gameObject.SetActive(false);
            var player = col.transform.parent.GetComponent<ElympicsBehaviour>();
            if(Elympics.Player != player.PredictableFor) return;
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
        CleanUI();
        for(int i=0; i<machineTasks.Count; i++)
        {
            var taskui = Instantiate(_TaskPrefab);
            taskui.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = machineTasks[i].Description;
            if (machineTasks[i].requirements.Count == 1)
                taskui.transform.GetChild(1).GetComponent<Image>().sprite = machineTasks[i].requirements[0].sprite;
            else
                taskui.transform.GetChild(1).gameObject.SetActive(false);
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
            taskui.transform.Find("TaskName").gameObject.GetComponent<TextMeshProUGUI>().text = machineSabotage[i].Description;
            if (machineSabotage[i].requirements.Count == 1)
                taskui.transform.Find("ToolIcon").gameObject.GetComponent<Image>().sprite = machineSabotage[i].requirements[0].sprite;
            else
                taskui.transform.Find("ToolIcon").gameObject.SetActive(false);
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

    private void CleanUI()
    {
        //Debug.Log("cleaning ui:" + TasksPanel.transform.childCount);
        while (TasksPanel.transform.childCount > 0)
        {
            Transform lastChild = TasksPanel.transform.GetChild(TasksPanel.transform.childCount - 1);
            lastChild.SetParent(null);
            Destroy(lastChild.gameObject);
        }
    }

    public void TaskClicked(int index)
    {
        currentPlayer.parent.GetComponent<Inputs>().inputStruct.taskID = index;
        //taskIndex.Value = index;
        //slider.maxValue = machineTasks[index].TaskTime;
        MachineUI.gameObject.SetActive(true);
        TasksPanel.gameObject.SetActive(false);
    }
    #endregion
}
