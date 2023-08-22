using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using UnityEditor;
using System;

public class InverseInputsBullet : ElympicsMonoBehaviour, IUpdatable
{
    public GothMovement gothMovement;
    private bool activate = false;
    public float time = 10f;
    public float duration = 15f;

    public void ElympicsUpdate()
    {
        if(activate)
        {
            Debug.Log("simsy");
            time = duration;
            activate = false;
            InverseInputs();
        }
        time -= Elympics.TickDuration;
        if (time < 0) 
        {
            gothMovement.inverted = 1;
            ElympicsDestroy(gameObject);
        }
    }

    private void InverseInputs()
    {
        gothMovement.inverted = -1;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //if(!Elympics.IsServer) return;
        // if(col.tag == "Machine") return;
        // if(col.tag == "Drawer") return;
        // if(col.tag == "Printer") return;
        // if(col.tag == "Computers") return;
        if(col.tag == "Player")
        {
            transform.SetParent(col.transform);
            gothMovement = col.transform.GetComponent<GothMovement>();
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            activate = true;
        }
        else if(col.tag == "Work")
        {
            transform.SetParent(col.transform.parent);
            gothMovement = col.transform.parent.GetComponent<GothMovement>();
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;

            activate = true;
        }
        else
            ElympicsDestroy(gameObject);
    }
}
