using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class TaskManager : ElympicsMonoBehaviour, IObservable
{
    private List<TaskData> playerTasks = new List<TaskData>();

    public List<ElympicsInt> myTasks = new List<ElympicsInt>();
    void Start()
    {
        if (!Elympics.IsServer) return;
        var machines = GameObject.FindGameObjectsWithTag("Machine");
        List<TaskData> allTasks = new List<TaskData>();
        foreach(GameObject machine in machines)
        {
            allTasks.AddRange(machine.GetComponent<Machine>().machineTasks);
        }
        Debug.Log(string.Join(", ", allTasks));
        var rand = Random.Range(0, allTasks.Count);
        playerTasks.Add(allTasks[rand]);
        myTasks.Add(new ElympicsInt());
        myTasks[0].Value = playerTasks[0].ID;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("id taska " + myTasks[0]);
    }
}
