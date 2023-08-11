using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using System;

public class GameInitializer : ElympicsMonoBehaviour, IInitializable
{
	[SerializeField] private float timeToStartMatch = 5.0f;
	public ElympicsFloat CurrentTimeToStartMatch { get; } = new ElympicsFloat(0.0f);

	private ElympicsBool gameInitializationEnabled = new(false);

	public ElympicsInt numberofPlayers = new(0);

	[SerializeField] private GameObject _GamePlayerPrefab;

    public void Initialize()
	{
		Debug.Log("nose bleed");
		// if(!Elympics.IsServer) return;

		var players = GameObject.FindGameObjectsWithTag("LobbyPlayer");
		foreach(GameObject player in players)
		{
			Debug.Log((int)Elympics.Player);
		}
		if((int)Elympics.Player == 0 || (int)Elympics.Player == 1)
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
				if(Elympics.IsServer)
				{
					Debug.Log(players.Length);
					// foreach(GameObject player in players)
					for(int i = 0; i < numberofPlayers.Value; i++)
					{
						var player = players[i];
						var ID = player.GetComponent<ElympicsBehaviour>().PredictableFor;
						var trans = player.transform;
						player.SetActive(false);
						GameObject gamePlayer = ElympicsInstantiate("GamePlayer", ElympicsPlayer.FromIndex((int)ID));
						gamePlayer.SetActive(true);
						gamePlayer.transform.position = trans.position;
					}
				}
				else
				{
					for(int i = 0; i < numberofPlayers.Value; i++)
					// foreach(GameObject player in players)
					{
						var player = players[i];
						player.SetActive(false);
						// if(Elympics.Player == player.GetComponent<ElympicsBehaviour>().PredictableFor)
						// {
						// 	try{
						// 		player.GetComponent<LobbyPlayer>().enabled = false;
						// 	}
						// 	catch{Debug.Log(player.name);}
						// 	//Destroy(player.GetComponent<LobbyPlayer>());
						// 	player.AddComponent<PlayerHandler>();
						// }
					}

					// Camera.main.GetComponent<CameraMove>().enabled = true;
				}


				
			}
		}
	}

}
