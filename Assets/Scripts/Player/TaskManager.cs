using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Elympics;

public class TaskManager : ElympicsMonoBehaviour, IObservable
{
    [SerializeField] private GameObject TaskPanel;
    [SerializeField] private int numberOfTasks = 2;
    
    public ElympicsBool done = new ElympicsBool(false);
    public ElympicsBool finished = new ElympicsBool(false);
    private ElympicsBool tasksReady = new ElympicsBool(false);
    public List<TaskData> allTasks = new List<TaskData>();
    private List<TaskData> playerTasks = new List<TaskData>();

    // list with tasks' IDs 
    // public List<ElympicsInt> myTasks = new List<ElympicsInt>();
    public ElympicsList<ElympicsInt> myTasks = new ElympicsList<ElympicsInt>(() => new ElympicsInt());

    public void Start()
    {
        tasksReady.ValueChanged += UpdateTaskUI;
        TaskPanel = GameObject.Find("MainUI").transform.Find("TaskPanel").gameObject;
        finished.ValueChanged += GameFinished;
        done.ValueChanged += TasksRecorded;
        //gather all tasks from all machines
        allTasks = FindAllTasks();
        //Debug.Log(string.Join(", ", allTasks));

        //randomly pick some tasks for player
        if (Elympics.IsServer) ChooseTasks(allTasks);   
        else if (Elympics.Player == PredictableFor){
            // while(tasksReady.Value == false) {}
            UpdateTaskUI(false, true);
        }
    }

    public void Update()
    {
        if (Elympics.Player != PredictableFor) return;
        if(myTasks.Count == 0) return;
        for (int i=0; i<myTasks.Count; i++)
        {
            if(myTasks[i].Value == -1)
            {
                TaskPanel.transform.GetChild(i+1).localScale *= 0.95f;
            }     
        }
        for (int i=0; i<myTasks.Count; i++)
        {
            if(myTasks[i].Value != -1)
            {
                return;
            }  
        }
        TaskPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.green;
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
            playerTasks.Add(allTasks[rand]);
            //synchronized lists of player's tasks' IDs
            myTasks.Add().Value = playerTasks[i].ID;
            // myTasks.Add(new ElympicsInt(playerTasks[i].ID));

            // OnValueChanged called only on server!! CHANGE!!!
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
        // copy.TaskTime = original.TaskTime;
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
            var ntask = Instantiate(child);
            ntask.transform.localScale = ntask.transform.localScale * 2;
            Debug.Log("ooooooooo" + ntask.transform.localScale);
            ntask.GetComponent<TextMeshProUGUI>().text = name;
            ntask.GetComponent<TextMeshProUGUI>().color = allTasks.Find(x => x.ID == id.Value).color;
            ntask.GetComponent<TextMeshProUGUI>().alpha = 255;
            ntask.transform.SetParent(TaskPanel.transform);
        }
    }

    public void OnTaskCompleted(int taskID) 
    {
        Debug.Log("ID of finished task: "+ taskID);
        try 
        {
            //playerTasks.Find(x => x.ID == taskID).Completed = true;
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

    private void TasksRecorded(bool lastValue, bool newValue)
    {
        if(Elympics.Player != PredictableFor) return;
        // TaskPanel.SetActive(false);
        StartCoroutine(TaskPanelAnim());

    }

    private IEnumerator TaskPanelAnim()
    {
        for(int i=10; i<50; i++)
        {
            Debug.Log("sensore");
            TaskPanel.transform.Translate(Vector3.down * 20/i);
            yield return new WaitForSeconds(0.01f);
        }
        for(int i=80; i>0; i--)
        {
            Debug.Log("sensore");
            TaskPanel.transform.Translate(Vector3.up * 20/i);
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void GameFinished(bool lastValue, bool newValue)
    {
        if(Elympics.Player != PredictableFor) return;
        Debug.Log("nothing wrong with me");

        var UI = TaskPanel.transform.parent;
        // var WIN = Instantiate(new GameObject(), UI.transform);
        var WIN = GameObject.Find("MainUI").transform.GetChild(9);
        WIN.gameObject.SetActive(true);
        // WIN.AddComponent<TextMeshProUGUI>();
        if (!AreTasksCompleted())
        {
            WIN.GetComponent<TextMeshProUGUI>().text = "YOUR WORK WAS IN VAIN";
            // WIN.GetComponent<TextMeshProUGUI>().aligment = TextAligment.Center;
            WIN.GetComponent<TextMeshProUGUI>().color = Color.blue;
        }
        else
        {
            WIN.GetComponent<TextMeshProUGUI>().text = "YOUR VAIN WON";
            // WIN.GetComponent<TextMeshProUGUI>().aligment = TextAligment.Center;
            WIN.GetComponent<TextMeshProUGUI>().color = Color.green;
        }
        TaskManager[] allPlayers = FindObjectsOfType<TaskManager>();
        foreach(TaskManager player in allPlayers)
        {
            if(!player.AreTasksCompleted()) 
            {
                Camera.main.GetComponent<CameraMove>().ChangeTarget(player.transform);
                return;
            }
        }

    }

    private void OnValueChanged(int lastValue, int newValue)
    {
        //Debug.Log("task completed, changed to -1");

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
