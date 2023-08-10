using System.Collections;
using System.Collections.Generic;
using Elympics;
using UnityEngine;

public class PlayersInitializer : MonoBehaviour, IServerHandlerGuid
{
    public void OnPlayerConnected(ElympicsPlayer player)
    {
        Debug.Log("bitch im here to kick your ass");
        GetComponent<GameInitializer>().numberofPlayers += 1;
    }

    public void OnPlayerDisconnected(ElympicsPlayer player)
    {
        Debug.Log("player disconnected " + player);
        GetComponent<GameInitializer>().numberofPlayers -= 1;
    }

    public void OnServerInit(InitialMatchPlayerDatasGuid initialMatchPlayerDatas)
    {
        Debug.Log("Dude i dont know");
    }
}
