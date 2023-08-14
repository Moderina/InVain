using UnityEngine;

public struct InputStruct 
{
    public int direction;
    public int jump;
    public int taskID;
    public bool work;
    public bool abilityKey;
    public int ability;
    public bool shoot;
    public bool wantsToFinish;
    public Vector3 mousePos;
}

public class Inputs : MonoBehaviour
{
    public InputStruct inputStruct;

    public void Start() {inputStruct.taskID = -1;}

    public void UpdateInput() 
    {
        inputStruct.mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        inputStruct.direction = (int)Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump")) inputStruct.jump=1;
        inputStruct.work = Input.GetKey(KeyCode.E);
        inputStruct.abilityKey = Input.GetKey(KeyCode.F);
        // inputStruct.shoot = Input.GetMouseButton(0);
        inputStruct.shoot = Input.GetButton("Fire1");
        inputStruct.wantsToFinish = Input.GetKey(KeyCode.Y);
    }

    public InputStruct GetInput() 
    {
        InputStruct returnstruct = inputStruct;
        inputStruct.direction = 0;
        inputStruct.jump = 0;
        inputStruct.taskID = -1;
        inputStruct.work = false;
        inputStruct.abilityKey = false;
        inputStruct.ability = -1;
        inputStruct.shoot = false;
        inputStruct.mousePos = transform.position + transform.right;
        return returnstruct;
    }
}
