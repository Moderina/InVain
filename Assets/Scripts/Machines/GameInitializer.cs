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

    public void Initialize()
	{
		Debug.Log("nose bleed");
		// if(!Elympics.IsServer) return;

		var players = GameObject.FindGameObjectsWithTag("Player");
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
			}
		}
	}

}
