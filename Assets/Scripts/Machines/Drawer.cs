using System;
using System.Collections;
using System.Collections.Generic;
using Elympics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Drawer : ElympicsMonoBehaviour, IObservable
{
    [SerializeField] private MachineLook machineLook;
    // [SerializeField] private Canvas ItemsUI;
    [SerializeField] private GameObject ItemsPanel;
    [SerializeField] private GameObject _ItemPrefab;

    public List<ItemData> drawerItems = new List<ItemData>();
    // private ElympicsInt[] avaiableItems;
    private ElympicsList<ElympicsInt> avaiableItems = new ElympicsList<ElympicsInt>(() => new ElympicsInt());
    private ElympicsInt itemIndex = new ElympicsInt(-1);

    private Transform currentPlayer;

    void Start()
    {
        //put random ammount of items in the drawer
        // avaiableItems = new ElympicsInt[drawerItems.Count];
        // Debug.Log("yyy " + avaiableItems.Length);
        // Debug.Log("uuu " + drawerItems.Count);
        // foreach (ItemData itemData in drawerItems)
        // {
        //     int num = UnityEngine.Random.Range(0,5);
        //     avaiableItems.Add(new ElympicsInt(num));
        // }
        // if(transform.name != "Tool Drawer") return;
        if(!Elympics.IsServer) return;
        Debug.Log("i am the server");
        for(int i=0; i<drawerItems.Count; i++)
        {
            int num = UnityEngine.Random.Range(0,5);
            avaiableItems.Add().Value = num;
            // avaiableItems[i] = new ElympicsInt(num);
            // avaiableItems[i].Value = num;
            Debug.Log("SSSSSSS" + avaiableItems[i].Value);
        }
        // ItemsUI.gameObject.SetActive(false);
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
            //load updated UI with currently avaiable items
            LoadItemUI();
            // ItemsUI.gameObject.SetActive(true);
            machineLook.OnMachineInteracted(false);
        }
    }

    //Server
    void OnTriggerStay2D(Collider2D col)
    {
        //dont let other players to interact now
        if (col.transform == currentPlayer) 
        {
            //check if item chosen
            //same variable is used to store taskID and itemID in player's script
            var taskID = currentPlayer.parent.GetComponent<PlayerHandler>().taskID;
            if (taskID != -1)
            {
                itemIndex.Value = taskID;
                currentPlayer.parent.GetComponent<PlayerHandler>().taskID = -1;
                Debug.Log("Taskindex: " + itemIndex);
                currentPlayer.GetComponentInParent<InventoryManager>().AddItem(itemIndex.Value);
                try {
                    var newindex = drawerItems.FindIndex(x => x.ID == itemIndex.Value);
                    avaiableItems[newindex].Value -= 1;
                }
                catch {}
                // avaiableItems[itemIndex.Value].Value = avaiableItems[itemIndex.Value].Value -1;
                itemIndex.Value = -1;
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (currentPlayer != null && col.transform == currentPlayer) 
        {
            currentPlayer = null;
            itemIndex.Value = -1;
            // UnloadItemUI();
            machineLook.OnMachineInteracted(true);
            // ItemsUI.gameObject.SetActive(false);
            Debug.Log("leftAREA");
        }
    }


    private void LoadItemUI()
    {
        Debug.Log("AAAAAAAAAAA" + drawerItems.Count);
        UnloadItemUI();
        for(int i=0; i<drawerItems.Count; i++)
        {
            if (avaiableItems[i].Value < 1) continue;
            var itemui = Instantiate(_ItemPrefab);
            // taskui.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>().text = drawerItems[i].Name;
            itemui.GetComponent<Image>().sprite = drawerItems[i].sprite;
            itemui.name = drawerItems[i].ID.ToString();
            Debug.Log("DDDDDDDDDDDDupa");
            itemui.transform.SetParent(ItemsPanel.transform);
            itemui.transform.localScale = itemui.transform.localScale * 2;
            itemui.GetComponent<Button>().onClick.AddListener(delegate() 
            {
                int.TryParse(itemui.name, out int index);
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
        machineLook.OnMachineInteracted(true);
        //taskIndex.Value = index;
        //slider.maxValue = machineTasks[index].TaskTime;
        // ItemsUI.gameObject.SetActive(false);
    }
}
