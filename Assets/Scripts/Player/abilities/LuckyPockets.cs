using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using Random = UnityEngine.Random;

public class LuckyPockets : ElympicsMonoBehaviour, IObservable, IUpdatable
{
    [SerializeField] private Actions actions;
    [SerializeField] private InventoryManager inv;
    public float coolDown = 20f;
    private ElympicsFloat lastUse = new ElympicsFloat(2f);
    void Start()
    {
        actions.OnAbilityTriggered += AbilityActivate;
    }

    private void AbilityActivate()
    {
        if (lastUse.Value > 0 || !Elympics.IsServer) return;
        int count = inv.GetItemsCount();
        inv.AddItem(Random.Range(0, count));
        lastUse.Value = coolDown;
    }

    // Update is called once per frame
    public void ElympicsUpdate()
    {
        lastUse.Value -= Elympics.TickDuration;
    }
}
