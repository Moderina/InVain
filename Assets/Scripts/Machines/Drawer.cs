using System;
using System.Collections;
using System.Collections.Generic;
using Elympics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Drawer : ElympicsMonoBehaviour, IObservable
{
    [SerializeField] private Canvas ItemsUI;
    [SerializeField] private GameObject ItemsPanel;
    [SerializeField] private GameObject _ItemPrefab;

    public List<ItemData> drawerItems = new List<ItemData>();
    private List<ElympicsInt> avaiableItems = new List<ElympicsInt>();
    private ElympicsInt itemIndex = new ElympicsInt(-1);

    private Transform currentPlayer;

    void Start()
    {
        foreach (ItemData itemData in drawerItems)
        {
            int num = UnityEngine.Random.Range(0,5);
            avaiableItems.Add(new ElympicsInt(num));
        }
        ItemsUI.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(currentPlayer == null && col.transform.tag == "Work") 
        {
            currentPlayer = col.transform;
            currentPlayer.Find("Canvas").Find("Missing").gameObject.SetActive(false);

            //turn on UI only for interacting player
            var player = currentPlayer.parent.GetComponent<ElympicsBehaviour>();
            if(Elympics.Player != player.PredictableFor) return;
            LoadItemUI();
            ItemsUI.gameObject.SetActive(true);
        }
    }

    //Server
    void OnTriggerStay2D(Collider2D col)
    {
        //dont let other players to interact now
        if (col.transform == currentPlayer) 
        {
            //check if item chosen
            var taskID = currentPlayer.parent.GetComponent<PlayerHandler>().taskID;
            if (taskID != -1)
            {
                itemIndex.Value = taskID;
                currentPlayer.parent.GetComponent<PlayerHandler>().taskID = -1;
                Debug.Log("Taskindex: " + itemIndex);
            }
            
            //if item chosen, let pick up
            if(itemIndex.Value != -1)
            {
                //currentPlayer.Find("Canvas").gameObject.SetActive(true);
                currentPlayer.GetComponentInParent<InventoryManager>().AddItem(itemIndex.Value);
                avaiableItems[itemIndex.Value].Value = avaiableItems[itemIndex.Value].Value -1;
                itemIndex.Value = -1;
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        // if (col.transform.tag == "Work") {
        if (currentPlayer != null && col.transform == currentPlayer) 
        {
            currentPlayer = null;
            itemIndex.Value = -1;
            UnloadItemUI();
            ItemsUI.gameObject.SetActive(false);
            Debug.Log("leftAREA");
        }
    }


    private void LoadItemUI()
    {
        for(int i=0; i<drawerItems.Count; i++)
        {
            if (avaiableItems[i].Value < 1) return;
            var taskui = Instantiate(_ItemPrefab);
            taskui.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>().text = drawerItems[i].Name;
            taskui.name = drawerItems[i].ID.ToString();
            taskui.transform.SetParent(ItemsPanel.transform);
            taskui.GetComponent<Button>().onClick.AddListener(delegate() 
            {
                int.TryParse(taskui.name, out int index);
                ItemClicked(index);
            });
        }
    }

    private void UnloadItemUI()
    {
        while(ItemsPanel.transform.childCount > 0)
        {
            Transform lastChild = ItemsPanel.transform.GetChild(ItemsPanel.transform.childCount - 1);
            lastChild.SetParent(null);
            Destroy(lastChild.gameObject);
        }
    }

    public void ItemClicked(int index)
    {
        currentPlayer.parent.GetComponent<Inputs>().inputStruct.taskID = index;
        //taskIndex.Value = index;
        //slider.maxValue = machineTasks[index].TaskTime;
        ItemsUI.gameObject.SetActive(false);
    }
}
