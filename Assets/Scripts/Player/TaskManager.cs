using System.Collections;
using System.Collections.Generic;
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
        //gather all tasks from all machines
        allTasks = FindAllTasks();
        Debug.Log(string.Join(", ", allTasks));

        if (Elympics.IsServer) ChooseTasks(allTasks);
        /*if (Elympics.Player == PredictableFor)*/ 
        //randomly pick some tasks for player
        

        //myTasks[0].Value = playerTasks[0].ID;


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

    // Update is called once per frame
    void Update()
    {
        if (Elympics.Player != PredictableFor) return;
        for (int i=0; i<myTasks.Count; i++)
        {
            if(myTasks[i].Value == -1)
            {
                TaskPanel.transform.GetChild(i+1).GetComponent<TextMeshProUGUI>().color = Color.green;
                myTasks[i].Value = -2;
            }
        }
        if(finished.Value) GameFinished();
    }

    public List<TaskData> FindAllTasks() 
    {
        var machines = GameObject.FindGameObjectsWithTag("Machine");
        List<TaskData> allTasks = new List<TaskData>();
        foreach(GameObject machine in machines)
        {
            allTasks.AddRange(machine.GetComponent<Machine>().machineTasks);
        }
        return allTasks;
    }

    public void ChooseTasks(List<TaskData> allTasks)
    {
        for (int i=0; i<numberOfTasks; i++)
        {
            var rand = Random.Range(0, allTasks.Count);
            playerTasks.Add(CreateCopy(allTasks[rand]));
            Debug.Log("rand: " + rand);
            //synchronized lists of player's tasks' IDs
            myTasks.Add().Value = playerTasks[i].ID;
            myTasks[i].ValueChanged += OnValueChanged;
        }
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

    public void OnTaskCompleted(int taskID) 
    {
        Debug.Log("ID of finished task: "+ taskID);
        try 
        {
            playerTasks.Find(x => x.ID == taskID).Completed = true;
            myTasks[playerTasks.FindIndex(x => x.ID == taskID)].Value = -1;

            foreach (ElympicsInt state in myTasks)
            {
                Debug.Log("state: "+ state.Value);
                if (state.Value != -1) return;
            }
            finished.Value = true;
            GameFinished();
        }
        catch{}
    }

    private void GameFinished()
    {
        GetComponent<PlayerHandler>().enabled = false;
        var UI = TaskPanel.transform.parent;
        var WIN = Instantiate(new GameObject(), UI.transform);
        WIN.AddComponent<TextMeshProUGUI>();
        WIN.GetComponent<TextMeshProUGUI>().text = "YOU WON";
        WIN.GetComponent<TextMeshProUGUI>().color = Color.green;
    }

    private void OnValueChanged(int lastValue, int newValue)
    {
        Debug.Log("task completed, changed to -1");

    }
}
