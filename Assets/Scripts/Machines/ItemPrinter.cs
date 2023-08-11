using System.Collections;
using System.Collections.Generic;
using Elympics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPrinter : ElympicsMonoBehaviour, IUpdatable
{
    [SerializeField] private Canvas ItemsUI;
    [SerializeField] private GameObject ItemsPanel;
    [SerializeField] private GameObject _ItemPrefab;

    public List<ItemData> allItems = new List<ItemData>();

    private ElympicsInt itemIndex = new ElympicsInt(-1);
    private float itemTime = 0f;
    private ElympicsBool itemReady = new ElympicsBool(false);

    private Transform currentPlayer;
    void Start()
    {
        LoadItemUI();
        ItemsUI.gameObject.SetActive(false);
    }

    public void ElympicsUpdate()
    {
        if (itemIndex.Value != -1)
        {
            if (itemTime < 0)
            {
                itemReady.Value = true;
            }
            else
            {
                itemTime -= Elympics.TickDuration;
                Debug.Log(itemTime);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.transform.tag == "Work") 
        {
            currentPlayer = col.transform;

            //turn on UI only for interacting player
            var player = currentPlayer.parent.GetComponent<ElympicsBehaviour>();
            if(Elympics.Player != player.PredictableFor) return;
            if(itemIndex.Value != -1) return;
            ItemsUI.gameObject.SetActive(true);
        }
    }
    
    void OnTriggerStay2D(Collider2D col)
    {
        if(col.transform.tag != "Work") return;
        //dont let other players to interact now
        // if (col.transform == currentPlayer) 
        // {
            //check if item chosen
            Debug.Log(col.name);
            Debug.Log(col.transform.parent.name);
            var taskID = col.transform.parent.GetComponent<PlayerHandler>().taskID;
            if (taskID != -1)
            {
                itemTime = allItems.Find(x => x.ID == taskID).printingTime;
                itemReady.Value = false;
                itemIndex.Value = taskID;
                col.transform.parent.GetComponent<PlayerHandler>().taskID = -1;
                Debug.Log("Taskindex: " + itemIndex);
            }
            
            //if item chosen, let pick up
            if(itemReady.Value)
            {
                currentPlayer.GetComponentInParent<InventoryManager>().AddItem(itemIndex.Value);
                itemReady.Value = false;
                itemIndex.Value = -1;
            }
        // }
    }

    private void LoadItemUI()
    {
        for(int i=0; i<allItems.Count; i++)
        {
            var taskui = Instantiate(_ItemPrefab);
            taskui.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>().text = allItems[i].Name;
            taskui.name = allItems[i].ID.ToString();
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
