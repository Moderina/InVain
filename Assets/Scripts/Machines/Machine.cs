using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Elympics;
using TMPro;
using System;

public class Machine : ElympicsMonoBehaviour, IObservable
{
    [SerializeField] private Canvas TasksUI;
    [SerializeField] private GameObject TasksPanel;
    [SerializeField] private GameObject _TaskPrefab;

    public List<TaskData> machineTasks = new List<TaskData>();

    private ElympicsInt taskIndex = new ElympicsInt(-1);
    private ElympicsFloat progress = new ElympicsFloat(0);

    Slider slider;
    private Transform currentPlayer;

    void Start()
    {
        LoadTaskUI();
        TasksUI.gameObject.SetActive(false);
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
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        //dont let other players to interact now
        if (col.transform == currentPlayer) 
        {
            //check if task chosen
            var taskID = currentPlayer.parent.GetComponent<PlayerHandler>().taskID;
            if (taskID != -1)
            {
                taskIndex.Value = taskID;
                currentPlayer.parent.GetComponent<PlayerHandler>().taskID = -1;
                //slider.maxValue = machineTasks[taskIndex.Value].TaskTime;
                slider.maxValue = machineTasks.Find(x => x.ID == taskIndex.Value).TaskTime;
                Debug.Log("Taskindex: " + taskIndex);
            }
            
            //if task chosen, let work
            if(taskIndex.Value != -1 && HasItems())
            {
                //currentPlayer.Find("Canvas").gameObject.SetActive(true);
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
            //col.transform.Find("Canvas").gameObject.SetActive(false);
            TasksUI.gameObject.SetActive(false);
            Debug.Log("leftAREA");
        }
    }

    private bool HasItems()
    {
        var requirements = machineTasks.Find(x => x.ID == taskIndex.Value).requirements;
        foreach(ItemData item in requirements)
        {
            if (!currentPlayer.GetComponentInParent<InventoryManager>().HasItem(item.ID)) return false;
        }
        return true;
    }

    private void RemoveItems()
    {
        var requirements = machineTasks.Find(x => x.ID == taskIndex.Value).requirements;
        foreach(ItemData item in requirements)
        {
            currentPlayer.GetComponentInParent<InventoryManager>().RemoveItem(item.ID);
        }
    }

    private void TaskCompleted()
    {
        currentPlayer.parent.GetComponent<TaskManager>().OnTaskCompleted(taskIndex.Value);
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
