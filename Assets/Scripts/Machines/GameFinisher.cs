using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using TMPro;
using System;

public class GameFinisher : ElympicsMonoBehaviour
{
    public TaskManager[] allPlayers = null;
    private Transform currentPlayer;
    private bool hasKey;
    [SerializeField] private GameObject Key;

    private ElympicsInt numberOfWinners = new ElympicsInt(0);

    public void Start()
    {
        allPlayers = FindObjectsOfType<TaskManager>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        
        if(currentPlayer == null && col.transform.tag == "Work") 
        {
            currentPlayer = col.transform;
            if (currentPlayer.Find("Key(Clone)")) hasKey = true;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.transform == currentPlayer)
        {
            if(currentPlayer.GetComponentInParent<PlayerHandler>().wantsToFinish && !hasKey)
            {
                if (!CanFinish()) return;
                hasKey = true;
                numberOfWinners.Value += 1;
                Win();
                currentPlayer.GetComponentInParent<PlayerHandler>().wantsToFinish = false;
                var key = ElympicsInstantiate("Key", ElympicsPlayer.All);
                key.AddComponent<Key>().OnCreate(currentPlayer);
            }
        }
    }

    private bool CanFinish()
    {
        return currentPlayer.GetComponentInParent<TaskManager>().AreTasksCompleted();
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (currentPlayer != null && col.transform == currentPlayer) {
            currentPlayer = null;
            hasKey = false;
        }
    }

    private void Win()
    {
        if (numberOfWinners.Value > 0) 
        {
            //currentPlayer.GetComponentInParent<PlayerHandler>().enabled = false;
            foreach(TaskManager player in allPlayers)
            {
                // if(player.AreTasksCompleted())
                // {
                    player.finished.Value = true;
                // }
            }
            this.gameObject.SetActive(false);
        }
    }
}
