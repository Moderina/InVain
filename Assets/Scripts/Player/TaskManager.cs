using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Elympics;

public class TaskManager : ElympicsMonoBehaviour, IObservable
{
    public GameObject TaskPanel;
    public int numberOfTasks = 1;
    
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
        //Debug.Log("id taska " + myTasks[0]);
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
            //myTasks[i].ValueChanged += OnValueChanged();
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
        //if()
    }

    
    private ElympicsVar<int>.ValueChangedCallback OnValueChanged()
    {
        throw new System.NotImplementedException();
    }
}
