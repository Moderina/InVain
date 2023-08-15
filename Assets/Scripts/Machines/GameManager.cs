using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using UnityEngine.SceneManagement;

public enum GameState
{
	Prematch = 0,
	GameplayMatchRunning,
	MatchEnded
}

public class GameManager : ElympicsMonoBehaviour, IInitializable
{
    public ElympicsInt CurrentGameState { get; } = new ElympicsInt((int)GameState.Prematch);
    [SerializeField] private GameObject QuitMenu;
    public void Initialize()
    {

    }

    public void ChangeGameState(GameState newGameState)
	{
		CurrentGameState.Value = (int)newGameState;
        GetComponent<GameFinisher>().allPlayers = FindObjectsOfType<TaskManager>();
        Debug.Log("NUMBER PLAYUERS: " + GetComponent<GameFinisher>().allPlayers.Length);
	}

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            QuitMenu.SetActive(!QuitMenu.activeSelf);
        }
    }

    public IEnumerator WaitToEnd()
    {
        ChangeGameState(GameState.MatchEnded);
        Debug.Log("ending game in 5seconds");
        yield return new WaitForSeconds(5);
        Elympics.EndGame();
    }

    public void Quit()
    {
        SceneManager.LoadScene(0);
    }
}
