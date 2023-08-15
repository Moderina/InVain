using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Elympics;
using System;

public class InventoryManager : ElympicsMonoBehaviour, IObservable
{
    [SerializeField] private GameObject Inventory;
    [SerializeField] private GameObject _prefab;
    private List<ItemData> allItems = new List<ItemData>();
    private ElympicsList<ElympicsInt> myItems = new ElympicsList<ElympicsInt>(() => new ElympicsInt());

    private ElympicsInt inventoryChange = new ElympicsInt(0);
    public void Start()
    {
        inventoryChange.ValueChanged += UpdateInventoryUI;
        Inventory = GameObject.Find("MainUI").transform.Find("ItemsPanel").gameObject;
        // var drawers = GameObject.FindGameObjectsWithTag("Drawer");
        // foreach(GameObject drawer in drawers)
        // {
        //     allItems.AddRange(drawer.GetComponent<Drawer>().drawerItems);
        // }
        allItems = GameObject.FindGameObjectWithTag("Printer").GetComponent<ItemPrinter>().allItems;
    }

    void Update()
    {
        //Debug.Log(string.Join(", ", myItems));
    }

    //Server
    public void AddItem(int id)
    {
        myItems.Add().Value = id;
        inventoryChange.Value +=1;
        //myItems.Add(new ElympicsInt(id));
    }

    public void RemoveItem(int id)
    {
        foreach(ElympicsInt item in myItems)
        {
            if(item.Value == id) 
            {
                myItems.Remove(item);
                inventoryChange.Value -=1;
                return;
            }
        }
    }

    public bool HasItem(int id)
    {
        return myItems.Any(x => x.Value == id);
    }

    private void UpdateInventoryUI(int lastValue, int newValue)
    {
        if(Elympics.Player != PredictableFor) return;
        if (newValue > lastValue)
        {
            var item = Instantiate(_prefab);
            Debug.Log("DUPA" + string.Join(",", allItems));
            Debug.Log(allItems.Find(x => x.ID == myItems.Last().Value));
            item.GetComponent<Image>().sprite = allItems.Find(x => x.ID == myItems.Last().Value).sprite;
            item.transform.SetParent(Inventory.transform);
        }
        else
        {
            for (int i = Inventory.transform.childCount - 1; i >= 0; i--)
            {
                GameObject child = Inventory.transform.GetChild(i).gameObject;
                Destroy(child);
            }
            foreach(ElympicsInt item in myItems)
            {
                var itemobject = Instantiate(_prefab);
                itemobject.GetComponent<Image>().sprite = allItems.Find(x => x.ID == item.Value).sprite;
                itemobject.transform.SetParent(Inventory.transform);
            }
        }
    }

    public int GetItemsCount()
    {
        return allItems.Count;
    }
}
