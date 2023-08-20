using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class sneakers : ElympicsMonoBehaviour, IUpdatable
{
    public Rigidbody2D rb;
    public float speed = 2;

    // Update is called once per frame
    public void ElympicsUpdate()
    {
        if (!Elympics.IsServer) return;
        if(rb == null)
            try
            {
                rb = transform.parent.GetComponent<Rigidbody2D>();
            }
            catch{}
        else
        {
            rb.velocity *= new Vector2(speed, 1);
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -15, 15), rb.velocity.y);
        }
    }
}
