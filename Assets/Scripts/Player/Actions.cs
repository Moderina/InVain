using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Actions : MonoBehaviour
{
    [SerializeField] private bool isWorking = false;
    [SerializeField] private Transform sprite;
    public bool abilityKey;
    public int ability;

    public delegate void AbilityEventDelegate();

    public event AbilityEventDelegate OnAbilityTriggered;

    public void UpdateActions(bool work) 
    {
        isWorking = work;
    }

    public bool IsWorking() {
        return isWorking;
    }


    public void UpdateShoot(bool shoot, UnityEngine.Vector3 mouse)
    {        
        if (sprite.childCount == 4 && sprite.GetChild(3).tag == "Weapon")
        {       
            sprite.GetChild(3).GetComponent<Pistol>().Shoot(mouse, shoot);
        }
    }

    public void UpdateAbility(bool abilityKey, int ability)
    {
        this.ability = ability;
        this.abilityKey = abilityKey;
        if (abilityKey) OnAbilityTriggered?.Invoke();
    }



}
