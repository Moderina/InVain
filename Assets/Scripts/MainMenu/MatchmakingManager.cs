using System;
using System.Collections;
using System.Collections.Generic;
using Elympics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class MatchmakingManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    private int numOfPlayers = 6;
    public Button joinBtn;
    public Button joinBtn2;
    [SerializeField] private TMP_InputField inputField;
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
            string queueName = GetQueueName();
            infoText.gameObject.SetActive(true);
            ElympicsLobbyClient.Instance.PlayOnlineInRegion(null, null, null, queueName);
            Debug.Log("im back");
        }
    }

    public void PlayCodeGame()
    {
        string code = inputField.text;
        Debug.Log(code);
        if (code != null && code != "")
        {
            if (joinBtn2.interactable == true)
            {
                string queueName = GetQueueName();
                infoText.gameObject.SetActive(true);
                ElympicsLobbyClient.Instance.PlayOnlineInRegion(null, null, null, $"{queueName}:{code}");
                Debug.Log($"{queueName}:{code}");
                Debug.Log("im back");
            }
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    private string GetQueueName()
    {
        switch(numOfPlayers)
        {
            case 0:
                return "six";
            case 1:
                return "five";
            case 2:
                return "four";
            case 3:
                return "three";
            case 4:
                return "two";
            default:
                return "six";
        }
    }

    private void DisplayMatchmakingStarted()
    {
        infoText.text = "Looking for match";
        joinBtn2.interactable = false;
        joinBtn.interactable = false;
    }

    
    private void DisplayMatchFound()
    {
        infoText.text = "Match found";
    }

    private void DisplayMatchmakingError()
    {
        infoText.text = "Couldnt join any match";
        joinBtn2.interactable = true;
        joinBtn.interactable = true;
    }

    public void DropdownChanged(int num)
    {
        numOfPlayers = num;
        Debug.Log(numOfPlayers);
        numOfPlayers = dropdown.value;
        Debug.Log(numOfPlayers);
    }
}
