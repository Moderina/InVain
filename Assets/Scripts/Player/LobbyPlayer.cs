using System;
using System.Collections;
using System.Collections.Generic;
using Elympics;
using UnityEngine;

public struct LobbyInputs
{
    public bool gameReady;
    public int playerclass;
}

public class LobbyPlayer : ElympicsMonoBehaviour, IInputHandler, IUpdatable, IObservable
{
    [SerializeField] private GameInitializer gameInitializer;

	[SerializeField] private Inputs inputs;
	[SerializeField] private GothMovement movement;
	[SerializeField] private Jump jump;

    public LobbyInputs lobbyInputs;
    public LobbyInputs lobbyInputsServer;
    public void Start()
    {
        lobbyInputs.gameReady = false;
        lobbyInputs.playerclass = 0;
    }

    public void Update() {
		if(Elympics.Player != PredictableFor) return;
		inputs.UpdateInput();
	}

    public void OnStartGameButtonClicked()
    {
        lobbyInputs.gameReady = true;
    }

    public void OnInputForClient(IInputWriter inputSerializer) {
        InputStruct currentInputs = inputs.GetInput();
		inputSerializer.Write(currentInputs.direction);
		inputSerializer.Write(currentInputs.jump);
		inputSerializer.Write(currentInputs.mousePos.x);
		inputSerializer.Write(currentInputs.mousePos.y);
		inputSerializer.Write(currentInputs.mousePos.z);
		inputSerializer.Write(lobbyInputs.gameReady);
		inputSerializer.Write(lobbyInputs.playerclass);
        lobbyInputs.gameReady = false;
	}


    //SERVER


    public void ElympicsUpdate()
    {
        InputStruct currentInput;
		currentInput.direction = 0;
		currentInput.jump = 0;
		currentInput.mousePos = transform.position + transform.right;

        if(ElympicsBehaviour.TryGetInput(PredictableFor, out var inputReader)) 
        {
            inputReader.Read(out currentInput.direction);
            inputReader.Read(out currentInput.jump);
			inputReader.Read(out float x);
			inputReader.Read(out float y);
			inputReader.Read(out float z);
			currentInput.mousePos = new Vector3(x,y,z);
            inputReader.Read(out lobbyInputs.gameReady);
            inputReader.Read(out lobbyInputs.playerclass);
		}
        movement.Movement(currentInput.direction, currentInput.jump, currentInput.mousePos);
		jump.OnJumpInput(currentInput.jump);
        
        if ((lobbyInputs.gameReady != lobbyInputsServer.gameReady) && lobbyInputs.gameReady == true)
        {
            Debug.Log("Starting game");
            StartGame();
        }
        if(lobbyInputs.playerclass != lobbyInputsServer.playerclass)
        {
            ChangePlayerClass(lobbyInputs.playerclass);
        }
        lobbyInputsServer.gameReady = lobbyInputs.gameReady;
        lobbyInputsServer.playerclass = lobbyInputs.playerclass;
    }

    private void ChangePlayerClass(int playerclass)
    {
        throw new NotImplementedException();
    }

    private void StartGame()
    {
        Debug.Log("bout to init");
        gameInitializer.InitializeMatch();
        lobbyInputsServer.gameReady = false;
        Debug.Log("inited");
    }

    public void OnInputForBot(IInputWriter inputSerializer)
    {
        throw new NotImplementedException();
    }
}
