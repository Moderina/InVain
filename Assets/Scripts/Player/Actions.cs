using System.Collections;
using System.Collections.Generic;
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


    public void UpdateShoot(bool shoot, Vector3 mouse)
    {
        if (shoot && sprite.childCount == 3 && sprite.GetChild(2).tag == "Weapon")
        {
            Debug.Log("inside");
            sprite.GetChild(2).GetComponent<Pistol>().Shoot(mouse);
        }
    }

    public void UpdateAbility(bool abilityKey, int ability)
    {
        this.ability = ability;
        this.abilityKey = abilityKey;
        if (abilityKey) OnAbilityTriggered?.Invoke();
    }
}
