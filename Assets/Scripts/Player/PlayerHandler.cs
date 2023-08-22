using UnityEngine;
using Elympics;
using System;
using TMPro;

public class PlayerHandler : ElympicsMonoBehaviour, IInputHandler, IUpdatable, IObservable
{
	[SerializeField] private Inputs inputs;
	[SerializeField] private GothMovement movement;
	[SerializeField] private Jump jump;
	[SerializeField] private Actions actions;
	[SerializeField] private AnimationStateController animationSC;
	public ElympicsString playername = new ElympicsString("");

	//TODO: przeniesc taskID i wantsToFinish do Actions.cs
    public int taskID = -1;
	public bool wantsToFinish = false;

	public void Start()
	{
		playername.ValueChanged += OnClientNameChanged;
		inputs = GetComponent<Inputs>();
		movement = GetComponent<GothMovement>();
		jump = GetComponent<Jump>();
		actions = GetComponent<Actions>();
		GetComponent<TaskManager>().enabled = true;
		GetComponent<InventoryManager>().enabled = true;
		transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = playername.Value;
		if(Elympics.Player != PredictableFor) return;
		Camera.main.GetComponent<CameraMove>().enabled = true;
	}

    public void Update() {
		if(Elympics.Player != PredictableFor) return;
		inputs.UpdateInput();
	}

	public void ElympicsUpdate() {
		InputStruct currentInput;
		currentInput.direction = 0;
		currentInput.jump = 0;
		currentInput.work = false;
		currentInput.abilityKey = false;
		currentInput.ability = -1;
		currentInput.shoot = false;
		wantsToFinish = false;
		currentInput.mousePos = transform.position + transform.right;

		if(ElympicsBehaviour.TryGetInput(PredictableFor, out var inputReader)) 
        {
            inputReader.Read(out currentInput.direction);
            inputReader.Read(out currentInput.jump);
            inputReader.Read(out taskID);
			inputReader.Read(out currentInput.work);
			inputReader.Read(out currentInput.abilityKey);
			inputReader.Read(out currentInput.ability);
			inputReader.Read(out currentInput.shoot);
			inputReader.Read(out wantsToFinish);
			inputReader.Read(out float x);
			inputReader.Read(out float y);
			inputReader.Read(out float z);
			currentInput.mousePos = new Vector3(x,y,z);
		}
		if (currentInput.work) Debug.Log("lov eme");
		movement.Movement(currentInput.direction, currentInput.jump, currentInput.mousePos);
		jump.OnJumpInput(currentInput.jump);
		actions.UpdateActions(currentInput.work);
		actions.UpdateShoot(currentInput.shoot, currentInput.mousePos);
		actions.UpdateAbility(currentInput.abilityKey, currentInput.ability);
	}

	public void OnInputForClient(IInputWriter inputSerializer) {
		InputStruct currentInputs = inputs.GetInput();
		inputSerializer.Write(currentInputs.direction);
		inputSerializer.Write(currentInputs.jump);
        inputSerializer.Write(currentInputs.taskID);
		inputSerializer.Write(currentInputs.work);
		inputSerializer.Write(currentInputs.abilityKey);
		inputSerializer.Write(currentInputs.ability);
		inputSerializer.Write(currentInputs.shoot);
		inputSerializer.Write(currentInputs.wantsToFinish);
		inputSerializer.Write(currentInputs.mousePos.x);
		inputSerializer.Write(currentInputs.mousePos.y);
		inputSerializer.Write(currentInputs.mousePos.z);
	}

	public void OnInputForBot(IInputWriter inputSerializer) {
		//throw new System.NotImplementedException();
	}

	
    private void OnClientNameChanged(string lastValue, string newValue)
    {
		Debug.Log("CHAGED");
        transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = newValue;
    }
}
