using System.Collections;
using System.Collections.Generic;
using Elympics;
using UnityEngine;
using UnityEngine.UI;

public class MatchmakingManager : MonoBehaviour
{
    public Button joinBtn;
    public void PlayOnline()
    {
        if (joinBtn.interactable == true)
        {
            ElympicsLobbyClient.Instance.PlayOnlineInRegion(null, null, null, "Default");
            joinBtn.interactable = false;
            Debug.Log("im back");
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
