using UnityEngine;

public struct GothInputStruct
{
    public int direction;
    public int jump;
    public bool work;
    public Vector3 mousePos;
}

public class GothInputs : MonoBehaviour
{
    public GothInputStruct inputStruct;

    public void UpdateGothInputStruct()
    {
        inputStruct.mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        inputStruct.direction = (int)Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump")) inputStruct.jump = 1;
        inputStruct.work = Input.GetKey(KeyCode.E);
    }

    public GothInputStruct GetGothInputStruct()
    {
        GothInputStruct returnGothInputStruct = inputStruct;
        inputStruct.direction = 0;
        inputStruct.jump = 0;
        inputStruct.work = false;
        inputStruct.mousePos = transform.position + transform.right;
        return returnGothInputStruct;
    }
}
