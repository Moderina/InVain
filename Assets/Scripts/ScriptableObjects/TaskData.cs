using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Machines/TaskData")]
public class TaskData : ScriptableObject
{
    private static int nextID = 0;
    public int ID; //{get; private set;}
    public string Description;

    public int TaskTime;

    private void OnEnable() 
    {
        ID = nextID++;
        Description = name;
    }
}
