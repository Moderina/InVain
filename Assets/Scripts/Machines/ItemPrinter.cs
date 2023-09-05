using System.Collections;
using System.Collections.Generic;
using Elympics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPrinter : ElympicsMonoBehaviour, IUpdatable
{
    [SerializeField] private PrinterLook printerLook;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private GameObject ItemsPanel;
    [SerializeField] private GameObject _ItemPrefab;

    public List<ItemData> allItems = new List<ItemData>();

    private ElympicsInt itemIndex = new ElympicsInt(-1);
    private ElympicsFloat itemTime = new ElympicsFloat(0f);
    private ElympicsBool itemReady = new ElympicsBool(false);

    private Transform currentPlayer;
    void Start()
    {
        LoadItemUI();
    }

    public void ElympicsUpdate()
    {
        if (itemIndex.Value != -1)
        {
            if (itemTime.Value < 0)
            {
                itemReady.Value = true;
                timer.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                itemTime.Value -= Elympics.TickDuration;
            }
        }
    }

    //UI updated hre cu doenst work in ElympicsUpdate for clients
    public void Update()
    {
        if (Elympics.IsServer) return;
        if (itemIndex.Value != -1)
        {
            if (itemTime.Value < 0)
            {
                timer.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                timer.text = ((int)itemTime.Value).ToString();
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
            // ItemsUI.gameObject.SetActive(true);
            // if((currentPlayer.parent.GetComponent<GothMovement>().faceDir == 1 && ItemsPanel.transform.position.x < 0)
            //  || (currentPlayer.parent.GetComponent<GothMovement>().faceDir == -1 && ItemsPanel.transform.position.x > 0))
            if ((currentPlayer.parent.GetComponent<GothMovement>().faceDir == 1 && ItemsPanel.GetComponent<RectTransform>().anchoredPosition.x < 0)
                || (currentPlayer.parent.GetComponent<GothMovement>().faceDir == -1 && ItemsPanel.GetComponent<RectTransform>().anchoredPosition.x > 0))
            {
                var pos = ItemsPanel.GetComponent<RectTransform>().anchoredPosition;
                pos.x *= -1;
                ItemsPanel.GetComponent<RectTransform>().anchoredPosition = pos;
            }
            printerLook.OnPrinterInteracted(false);
        }
    }
    
    void OnTriggerStay2D(Collider2D col)
    {
        if(col.transform.tag != "Work") return;
        if ((currentPlayer.parent.GetComponent<GothMovement>().faceDir == 1 && ItemsPanel.GetComponent<RectTransform>().anchoredPosition.x < 0)
            || (currentPlayer.parent.GetComponent<GothMovement>().faceDir == -1 && ItemsPanel.GetComponent<RectTransform>().anchoredPosition.x > 0))
        {
            var pos = ItemsPanel.GetComponent<RectTransform>().anchoredPosition;
            pos.x *= -1;
            ItemsPanel.GetComponent<RectTransform>().anchoredPosition = pos;
        }
        if(itemIndex.Value != -1 && itemReady.Value == false) 
        {
            timer.transform.parent.gameObject.SetActive(true);
            return;
        }
        currentPlayer = col.transform;
        //check if item chosen
        var taskID = col.transform.parent.GetComponent<PlayerHandler>().taskID;
        if (taskID != -1)
        {
            itemTime.Value = allItems.Find(x => x.ID == taskID).printingTime;
            itemReady.Value = false;
            itemIndex.Value = taskID;
            col.transform.parent.GetComponent<PlayerHandler>().taskID = -1;
            Debug.Log("ITEMindex: " + itemIndex);
        }
        
        //if item ready, let pick up
        if(itemReady.Value)
        {
            Debug.Log("ill give you");
            col.transform.GetComponentInParent<InventoryManager>().AddItem(itemIndex.Value);
            itemReady.Value = false;
            itemIndex.Value = -1;
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if(col.transform.tag != "Work") return;
        var player = col.transform.parent.GetComponent<ElympicsBehaviour>();
        if(Elympics.Player != player.PredictableFor) return;
        // ItemsUI.gameObject.SetActive(false);
        printerLook.OnPrinterInteracted(true);
    }

    private void LoadItemUI()
    {
        for(int i=0; i<allItems.Count; i++)
        {
            var itemui = Instantiate(_ItemPrefab);
            Debug.Log("item " + allItems[i].Name);
            // taskui.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>().text = allItems[i].Name;
            itemui.GetComponent<Image>().sprite = allItems[i].sprite;
            itemui.name = allItems[i].ID.ToString();
            itemui.transform.SetParent(ItemsPanel.transform);
            itemui.GetComponent<Button>().onClick.AddListener(delegate() 
            {
                int.TryParse(itemui.name, out int index);
                ItemClicked(index);
            });
        }
    }

    public void ItemClicked(int index)
    {
        currentPlayer.parent.GetComponent<Inputs>().inputStruct.taskID = index;
        // ItemsUI.gameObject.SetActive(false);
        printerLook.OnPrinterInteracted(true);
    }
}
