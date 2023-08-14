using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyBreaker : MonoBehaviour
{
    [SerializeField] private Actions actions;
    void Start()
    {
        actions.OnAbilityTriggered += AbilityActivate;
    }

    private void AbilityActivate()
    {
        Vector2 size = new Vector2(1f, 1f);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, size / 2f, 0f);
        foreach (Collider2D collider in colliders)
        {
            if(collider.tag == "Machine")
            {
                collider.GetComponent<Machine>().prankBroken.Value = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 size = new Vector3(1f, 1f, 1f);
        Gizmos.DrawWireCube(transform.position, size);
    }
}
