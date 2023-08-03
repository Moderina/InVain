using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public enum GameState
{
	Prematch = 0,
	GameplayMatchRunning,
	MatchEnded
}

public class GameManager : ElympicsMonoBehaviour, IInitializable
{
    public ElympicsInt CurrentGameState { get; } = new ElympicsInt((int)GameState.Prematch);
    public void Initialize()
    {

    }

    private void ChangeGameState(GameState newGameState)
	{
		CurrentGameState.Value = (int)newGameState;
	}

    public IEnumerator WaitToEnd()
    {
        ChangeGameState(GameState.MatchEnded);
        yield return new WaitForSeconds(5);
        Elympics.EndGame();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
