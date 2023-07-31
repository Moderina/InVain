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
    private ElympicsInt itemIndex = new ElympicsInt(-1);

    private Transform currentPlayer;

    void Start()
    {
        LoadItemUI();
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
            ItemsUI.gameObject.SetActive(true);
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        //dont let other players to interact now
        if (col.transform == currentPlayer) 
        {
            //check if task chosen
            var taskID = currentPlayer.parent.GetComponent<PlayerHandler>().taskID;
            if (taskID != -1)
            {
                itemIndex.Value = taskID;
                currentPlayer.parent.GetComponent<PlayerHandler>().taskID = -1;
                Debug.Log("Taskindex: " + itemIndex);
            }
            
            //if task chosen, let work
            if(itemIndex.Value != -1)
            {
                //currentPlayer.Find("Canvas").gameObject.SetActive(true);
                currentPlayer.GetComponentInParent<InventoryManager>().AddItem(itemIndex.Value);
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
            ItemsUI.gameObject.SetActive(false);
            Debug.Log("leftAREA");
        }
    }


    private void LoadItemUI()
    {
        for(int i=0; i<drawerItems.Count; i++)
        {
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

    public void ItemClicked(int index)
    {
        currentPlayer.parent.GetComponent<Inputs>().inputStruct.taskID = index;
        //taskIndex.Value = index;
        //slider.maxValue = machineTasks[index].TaskTime;
        ItemsUI.gameObject.SetActive(false);
    }
}
