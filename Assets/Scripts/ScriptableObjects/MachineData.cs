using System.Collections;
using System.Collections.Generic;
using Elympics;
using UnityEngine;

[CreateAssetMenu(menuName = "Machines/MechineData")]
public class MachineData : ScriptableObject
{
    public ElympicsBool isBroken = new ElympicsBool(false);

    // public ElympicsBool requireItem = new ElympicsBool(false);

    public ElympicsInt timeDown = new ElympicsInt(5);

    // public ElympicsInt itemID = new ElympicsInt(0);
}
