using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using System;

public class MachineLook : ElympicsMonoBehaviour, IObservable
{
    [SerializeField] private Machine machine;
    [SerializeField] private SpriteRenderer spriteRenderer;
    void Start()
    {
        // if(Elympics.IsServer) return;
        machine.isBroken.ValueChanged += MachineStateChanged;
    }

    public void MachineStateChanged(bool lastValue, bool newValue)
    {
        if (newValue)
        {
            spriteRenderer.material.color = new Color(255, 158, 158, 255);
        }
        else 
        {
            spriteRenderer.material.color = Color.white;
        }
    }
}
