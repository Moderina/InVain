using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Elympics;
using System;

public class PlayersWorkInfo : ElympicsMonoBehaviour
{
    [SerializeField] private GameObject canvas;
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag != "Work") return;
        var player = col.transform.parent.GetComponent<ElympicsBehaviour>();
        if(Elympics.Player != player.PredictableFor) return;
        canvas.SetActive(true);
        UpdateUIInfo();
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.tag != "Work") return;
        var player = col.transform.parent.GetComponent<ElympicsBehaviour>();
        if(Elympics.Player != player.PredictableFor) return;
        canvas.SetActive(false);
        ClearUIInfo();
    }


    private void UpdateUIInfo()
    {
        var infoTable = canvas.transform.GetChild(0);
        var playerCard = infoTable.GetChild(0);
        var gamePlayers = GameObject.FindGameObjectsWithTag("Player");
        // infoTable.GetChild(0).gameObject.SetActive(false);
        foreach(GameObject player in gamePlayers)
        {
            var nPlayerCard = Instantiate(playerCard);
            nPlayerCard.gameObject.SetActive(true);
            nPlayerCard.GetChild(1).GetComponent<TextMeshProUGUI>().text = player.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text;
            var alltasks = player.GetComponent<TaskManager>().allTasks;
            var playerTasks = player.GetComponent<TaskManager>().myTasks;
            var taskPanel = nPlayerCard.GetChild(2).GetChild(0);
            foreach(ElympicsInt id in playerTasks)
            {
                if(id.Value != -1)
                {
                    var nTask = Instantiate(taskPanel);
                    nTask.gameObject.SetActive(true);
                    nTask.GetComponent<TextMeshProUGUI>().text = alltasks.Find(x => x.ID == id.Value).Description;
                    nTask.SetParent(nPlayerCard.GetChild(2));
                }
            }
            // nPlayerCard.GetChild(2).GetChild(0).gameObject.SetActive(false);
            nPlayerCard.SetParent(infoTable);
        }
    }

    
    private void ClearUIInfo()
    {
        var infoTable = canvas.transform.GetChild(0);
        while(infoTable.childCount > 1)
        {
            Transform lastChild = infoTable.GetChild(infoTable.childCount - 1);
            lastChild.SetParent(null);
            Destroy(lastChild.gameObject);
        }
    }
}
