using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using System;

public class GameInitializer : ElympicsMonoBehaviour, IUpdatable, IInitializable
{
	[SerializeField] private float timeToStartMatch = 5.0f;
	public ElympicsFloat CurrentTimeToStartMatch { get; } = new ElympicsFloat(0.0f);

	private Action OnMatchInitializedAssignedCallback = null;

	private ElympicsBool gameInitializationEnabled = new(false);

	public ElympicsBool isStarting = new(false);

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
		if((int)Elympics.Player == 0)
		{
			GameObject.Find("MainUI").transform.Find("StartGame").gameObject.SetActive(true);
		}
	}

	//public void InitializeMatch(Action OnMatchInitializedCallback)
	public void InitializeMatch()
	{
		//OnMatchInitializedAssignedCallback = OnMatchInitializedCallback;
		Debug.Log("bleed");
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
				OnMatchInitializedAssignedCallback?.Invoke();

				gameInitializationEnabled.Value = false;
				GameObject.Find("Lobby").SetActive(false);

				GameObject[] players = GameObject.FindGameObjectsWithTag("LobbyPlayer");
				if(Elympics.IsServer)
				{
					Debug.Log(players.Length);
					foreach(GameObject player in players)
					{
						var ID = player.GetComponent<ElympicsBehaviour>().PredictableFor;
						player.SetActive(false);
						GameObject gamePlayer = ElympicsInstantiate("GamePlayer", ElympicsPlayer.FromIndex((int)ID));

						// try{
						// 	player.GetComponent<LobbyPlayer>().enabled = false;
						// }
						// catch{Debug.Log(player.name);}
						// //Destroy(player.GetComponent<LobbyPlayer>());
						// player.AddComponent<PlayerHandler>();
					}
				}
				else
				{
					foreach(GameObject player in players)
					{
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
