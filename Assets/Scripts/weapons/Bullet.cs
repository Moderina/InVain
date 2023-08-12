using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class Bullet : ElympicsMonoBehaviour, IUpdatable
{
    public Rigidbody2D rb;
    public float time = 10f;
    public int power = 2;

    public void ElympicsUpdate()
    {
        Debug.Log("choking me");
        time -= Elympics.TickDuration;
        if (time < 0) ElympicsDestroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Machine") return;
        if(col.tag == "Drawer") return;
        if(col.tag == "Printer") return;
        if(col.tag == "Computers") return;
        Debug.Log(rb.velocity);
        Vector2 force = new Vector2(Mathf.Sign(rb.velocity.x), 0);
        if(col.tag == "Player")
        {
            col.GetComponent<Rigidbody2D>().AddForce(force * power, ForceMode2D.Impulse);
        }
        if(col.tag == "Work")
        {
            col.transform.parent.GetComponent<Rigidbody2D>().AddForce(force * power, ForceMode2D.Impulse);
        }
        ElympicsDestroy(gameObject);
    }
}
