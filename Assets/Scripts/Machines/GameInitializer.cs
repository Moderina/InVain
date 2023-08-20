using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using System;

public class GameInitializer : ElympicsMonoBehaviour, IUpdatable, IInitializable
{
	[SerializeField] private GameEffects gameEffects;
	[SerializeField] private float timeToStartMatch = 5.0f;
	public ElympicsFloat CurrentTimeToStartMatch { get; } = new ElympicsFloat(100.0f);

	private ElympicsBool gameInitializationEnabled = new(false);

	public ElympicsInt numberofPlayers = new(0);

	[SerializeField] private GameObject _GamePlayerPrefab;

    public void Initialize()
	{
		// if(!Elympics.IsServer) return;

		var players = GameObject.FindGameObjectsWithTag("LobbyPlayer");
		if((int)Elympics.Player == 0)
		{
			GameObject.Find("MainUI").transform.Find("StartGame").gameObject.SetActive(true);
		}
	}

	//public void InitializeMatch(Action OnMatchInitializedCallback)
	public void InitializeMatch()
	{
		if(numberofPlayers.Value < 1) return;
		gameInitializationEnabled.Value = true;
		CurrentTimeToStartMatch.Value = timeToStartMatch;
		GameObject.Find("MainUI").transform.Find("StartGame").gameObject.SetActive(false);
	}

    // Update is called once per frame
    public void ElympicsUpdate()
	{
		if (gameInitializationEnabled.Value)
		{
			CurrentTimeToStartMatch.Value -= Elympics.TickDuration;
			if (CurrentTimeToStartMatch.Value < 0.0f)
			{
				gameInitializationEnabled.Value = false;
				GameObject.Find("Lobby").transform.Find("RightWall").gameObject.SetActive(false);
				GameObject.Find("Lobby").transform.Find("LeftWall").gameObject.SetActive(false);

				GameObject[] players = GameObject.FindGameObjectsWithTag("LobbyPlayer");
				players = Sort(players);
				// foreach(GameObject player in players)
				// {
				// 	Debug.Log(player.name + player.GetComponent<ElympicsBehaviour>().PredictableFor);
				// }
				if(Elympics.IsServer)
				{
					// foreach(GameObject player in players)
					for(int i = 0; i < numberofPlayers.Value; i++)
					{
						var player = players[i];
						var ID = player.GetComponent<ElympicsBehaviour>().PredictableFor;
						// Debug.Log(i + ": PredictableFor " + ID);
						var trans = player.transform;
						player.GetComponent<ElympicsBehaviour>().enabled = false;
						player.SetActive(false);
						GameObject gamePlayer = ElympicsInstantiate("Scientist_PartyBreaker", ElympicsPlayer.FromIndex((int)ID));
						gamePlayer.SetActive(true);
						gamePlayer.transform.position = trans.position;
					}
					GetComponent<GameManager>().ChangeGameState(GameState.GameplayMatchRunning);
				}
				else
				{
					for(int i = 0; i < numberofPlayers.Value; i++)
					// foreach(GameObject player in players)
					{
						var player = players[i];
						player.SetActive(false);
					}
				}


				
			}
		}
	}

	private GameObject[] Sort(GameObject[] players)
	{
		for(int i = 0; i < players.Length; i++)
		{
			for(int j = i+1; j < players.Length; j++)
			{
				if ((int)players[j].GetComponent<ElympicsBehaviour>().PredictableFor < (int)players[i].GetComponent<ElympicsBehaviour>().PredictableFor)
				{
                    (players[j], players[i]) = (players[i], players[j]);
                }
            }
		}
		return players;
	}

}
