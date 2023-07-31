using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Elympics;

public class InventoryManager : MonoBehaviour
{
    private List<ItemData> allItems;
    private List<ElympicsInt> myItems = new List<ElympicsInt>();

    public void AddItem(int id)
    {
        myItems.Add(new ElympicsInt(id));
    }

    public bool HasItem(int id)
    {
        return myItems.Any(x => x.Value == id);
    }
}
