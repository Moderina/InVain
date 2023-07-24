using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Elympics;

public class TaskManager : ElympicsMonoBehaviour, IObservable
{
    public GameObject TaskPanel;
    public int numberOfTasks = 3;
    
    public ElympicsBool finished = new ElympicsBool(false);
    private ElympicsBool tasksReady = new ElympicsBool(false);
    public List<TaskData> allTasks = new List<TaskData>();
    private List<TaskData> playerTasks = new List<TaskData>();

    public ElympicsList<ElympicsInt> myTasks = new ElympicsList<ElympicsInt>(() => new ElympicsInt());
    
    void Start()
    {
        tasksReady.ValueChanged += UpdateTaskUI;
        finished.ValueChanged += GameFinished;
        //gather all tasks from all machines
        allTasks = FindAllTasks();
        Debug.Log(string.Join(", ", allTasks));

        //randomly pick some tasks for player
        if (Elympics.IsServer) ChooseTasks(allTasks);   
    }

    void Update()
    {
        if (Elympics.Player != PredictableFor) return;
        for (int i=0; i<myTasks.Count; i++)
        {
            if(myTasks[i].Value == -1)
            {
                Debug.Log("fiufiu");
                TaskPanel.transform.GetChild(i+1).GetComponent<TextMeshProUGUI>().color = Color.green;
                //myTasks[i].Value = -2;
            }
        }
        //if(finished.Value) GameFinished();
    }

    public List<TaskData> FindAllTasks() 
    {
        //find all machines in scene
        var machines = GameObject.FindGameObjectsWithTag("Machine");
        List<TaskData> allTasks = new List<TaskData>();
        foreach(GameObject machine in machines)
        {
            allTasks.AddRange(machine.GetComponent<Machine>().machineTasks);
        }
        //return list with all avaiable tasks on scene
        return allTasks;
    }

    public void ChooseTasks(List<TaskData> allTasks)
    {
        for (int i=0; i<numberOfTasks; i++)
        {
            var rand = Random.Range(0, allTasks.Count);
            //dont allow two identical tasks
            if (playerTasks.Any(item => item.ID == allTasks[rand].ID))
            {
                i--;
                continue;
            }
            playerTasks.Add(CreateCopy(allTasks[rand]));
            Debug.Log("rand: " + rand);
            //synchronized lists of player's tasks' IDs
            myTasks.Add().Value = playerTasks[i].ID;
            myTasks[i].ValueChanged += OnValueChanged;
        }
        //let know clients that tasks been assigned so they can update theirs UI
        tasksReady.Value = true;
    }

    public TaskData CreateCopy(TaskData original)
    {
        TaskData copy = ScriptableObject.CreateInstance<TaskData>();
        copy.ID = original.ID;
        copy.Description = original.Description;
        copy.TaskTime = original.TaskTime;
        copy.Completed = false;
        return copy;
    }

    
    private void UpdateTaskUI(bool lastValue, bool newValue)
    {
        if (Elympics.Player != PredictableFor) return;
        TaskPanel.SetActive(true);
        var child = TaskPanel.transform.GetChild(0);
        foreach (ElympicsInt id in myTasks)
        {
            string name = allTasks.Find(x => x.ID == id.Value).Description;
            Debug.Log(name);
            var ntask = Instantiate(child);
            ntask.GetComponent<TextMeshProUGUI>().text = name;
            ntask.transform.SetParent(TaskPanel.transform);
        }
    }

    public void OnTaskCompleted(int taskID) 
    {
        Debug.Log("ID of finished task: "+ taskID);
        try 
        {
            playerTasks.Find(x => x.ID == taskID).Completed = true;
            myTasks[playerTasks.FindIndex(x => x.ID == taskID)].Value = -1;

            // foreach (ElympicsInt state in myTasks)
            // {
            //     Debug.Log("state: "+ state.Value);
            //     if (state.Value != -1) return;
            // }
            // finished.Value = true;
            //GameFinished();
        }
        catch{}
    }

    private void GameFinished(bool lastValue, bool newValue)
    {
        if(Elympics.Player != PredictableFor) return;
        var UI = TaskPanel.transform.parent;
        var WIN = Instantiate(new GameObject(), UI.transform);
        WIN.AddComponent<TextMeshProUGUI>();
        if (!AreTasksCompleted())
        {
            WIN.GetComponent<TextMeshProUGUI>().text = "YOU LOST";
            WIN.GetComponent<TextMeshProUGUI>().color = Color.blue;
        }
        else
        {
            WIN.GetComponent<TextMeshProUGUI>().text = "YOU WON";
            WIN.GetComponent<TextMeshProUGUI>().color = Color.green;
        }
        //GetComponent<PlayerHandler>().enabled = false;


    }

    private void OnValueChanged(int lastValue, int newValue)
    {
        Debug.Log("task completed, changed to -1");

    }

    public bool AreTasksCompleted()
    {
        for(int i = 0; i< myTasks.Count; i++)
        {
            if(myTasks[i].Value != -1) return false;
        }
        return true;
    }
}
