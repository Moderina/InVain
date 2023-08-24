using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        if(Input.GetKeyDown(KeyCode.E)) inputStruct.work = true;
        inputStruct.abilityKey = Input.GetKey(KeyCode.F);
        // inputStruct.shoot = Input.GetMouseButton(0);
        inputStruct.shoot = Input.GetButton("Fire1");
        inputStruct.wantsToFinish = Input.GetKey(KeyCode.Y);
        if(OverUI(Input.mousePosition)) inputStruct.shoot = false;
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

        private bool OverUI(UnityEngine.Vector3 mouse)
    {
        // Ray ray = Camera.main.ScreenPointToRay(mouse);
        // RaycastHit hitInfo;
        
        // LayerMask layerMask = 5;
        // if(Physics.Raycast(ray, out hitInfo, layerMask))
        // {
        //     Debug.Log(hitInfo.transform.name);
        //     if(hitInfo.transform.tag == "UI") return true;
        // }
        // return false;

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = mouse;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        for (int index = 0; index < raysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = raysastResults[index];
            if (curRaysastResult.gameObject.layer == 5)
                return true;
        }
        return false;
    }
}
