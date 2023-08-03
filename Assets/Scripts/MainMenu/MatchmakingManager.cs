using System;
using System.Collections;
using System.Collections.Generic;
using Elympics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatchmakingManager : MonoBehaviour
{
    public Button joinBtn;
    [SerializeField] private TextMeshProUGUI infoText;

    private void Start()
    {
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingStarted += DisplayMatchmakingStarted;
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingMatchFound += _ => DisplayMatchFound();
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingFailed += _ => DisplayMatchmakingError();
    }

    private void OnDestroy()
    {
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingStarted -= DisplayMatchmakingStarted;
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingMatchFound -= _ => DisplayMatchFound();
        ElympicsLobbyClient.Instance.Matchmaker.MatchmakingFailed -= _ => DisplayMatchmakingError();
    }
    
    public void PlayOnline()
    {
        if (joinBtn.interactable == true)
        {
            infoText.gameObject.SetActive(true);
            ElympicsLobbyClient.Instance.PlayOnlineInRegion(null, null, null, "Default");
            joinBtn.interactable = false;
            Debug.Log("im back");
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void DisplayMatchmakingStarted()
    {
        infoText.text = "Looking for match";
    }

    
    private void DisplayMatchFound()
    {
        infoText.text = "Match found";
    }

    private void DisplayMatchmakingError()
    {
        infoText.text = "Couldnt join any match";
    }
}
