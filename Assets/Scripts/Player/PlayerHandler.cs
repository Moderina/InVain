using UnityEngine;
using Elympics;

public class PlayerHandler : ElympicsMonoBehaviour, IInputHandler, IUpdatable {
	[SerializeField] private Inputs inputs;
	[SerializeField] private GothMovement movement;
	[SerializeField] private Actions actions;
    public int taskID = -1;

	private void Update() {
		if(Elympics.Player != PredictableFor) return;
		inputs.UpdateInput();
	}

	public void ElympicsUpdate() {
		InputStruct currentInput;
		currentInput.direction = 0;
		currentInput.jump = 0;
		currentInput.work = false;
		currentInput.mousePos = transform.position + transform.right;

		if(ElympicsBehaviour.TryGetInput(PredictableFor, out var inputReader)) 
        {
			float x,y,z;
			inputReader.Read(out currentInput.direction);
			inputReader.Read(out currentInput.jump);
            inputReader.Read(out taskID);
			inputReader.Read(out currentInput.work);
			inputReader.Read(out x);
			inputReader.Read(out y);
			inputReader.Read(out z);
			currentInput.mousePos = new Vector3(x,y,z);
		}

		movement.Movement(currentInput.direction, currentInput.jump, currentInput.mousePos);
		actions.UpdateActions(currentInput.work);
	}

	public void OnInputForClient(IInputWriter inputSerializer) {
		InputStruct currentInputs = inputs.GetInput();
		inputSerializer.Write(currentInputs.direction);
		inputSerializer.Write(currentInputs.jump);
        inputSerializer.Write(currentInputs.taskID);
		inputSerializer.Write(currentInputs.work);
		inputSerializer.Write(currentInputs.mousePos.x);
		inputSerializer.Write(currentInputs.mousePos.y);
		inputSerializer.Write(currentInputs.mousePos.z);
	}

	public void OnInputForBot(IInputWriter inputSerializer) {
		//throw new System.NotImplementedException();
	}
}
