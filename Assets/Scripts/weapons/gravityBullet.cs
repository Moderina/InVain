using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using UnityEditor;
using System;

public class gravityBullet : ElympicsMonoBehaviour, IUpdatable
{
    public Rigidbody2D rb;
    private bool activate = false;
    public float time = 10f;
    public float duration = 15f;

    public void ElympicsUpdate()
    {
        if(activate)
        {
            time = duration;
            activate = false;
            SetGravity();
        }
        time -= Elympics.TickDuration;
        if (time < 0) 
        {
            if(rb != null)
                rb.gravityScale = 1;
            ElympicsDestroy(gameObject);
        }
    }

    private void SetGravity()
    {
        rb.gravityScale = 0;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(!Elympics.IsServer) return;
        // if(col.tag == "Machine") return;
        // if(col.tag == "Drawer") return;
        // if(col.tag == "Printer") return;
        // if(col.tag == "Computers") return;
        if(col.tag == "Player")
        {
            transform.SetParent(col.transform);
            rb = col.transform.GetComponent<Rigidbody2D>();
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            activate = true;
        }
        else if(col.tag == "Work")
        {
            transform.SetParent(col.transform.parent);
            rb = col.transform.parent.GetComponent<Rigidbody2D>();
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;

            activate = true;
        }
        else
            ElympicsDestroy(gameObject);
    }
}
