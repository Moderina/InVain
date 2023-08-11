using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Machines/ItemData")]
public class ItemData : ScriptableObject
{
    public int ID;
    public string Name;
    public Sprite sprite;
    public int printingTime;
}
