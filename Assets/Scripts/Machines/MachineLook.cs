using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using System;

public class MachineLook : ElympicsMonoBehaviour, IObservable
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void ShowMachineUI(GameObject taskPanel)
    {
        Debug.Log(taskPanel.transform.position);
        //taskPanel.transform.position = new Vector3()
    }
}
