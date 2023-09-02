using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Machines/SabotageData")]
public class SabotageData : ScriptableObject
{
    public int ID; //{get; private set;}
    public string Description;
    public int TaskTime;

    public float width;
    public int ammount;

    public List<ItemData> requirements = new List<ItemData>();
}
