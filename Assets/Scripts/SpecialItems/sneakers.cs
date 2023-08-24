using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using UnityEngine.UI;

public class sneakers : ElympicsMonoBehaviour, IUpdatable
{
    public Image itemSprite, cooldown;
    public Rigidbody2D rb;
    public float speed = 2;
    public ElympicsBool taken = new ElympicsBool(false);
    public ElympicsFloat duration = new ElympicsFloat(10);

    public void Start()
    {
        itemSprite = GameObject.Find("MainUI").transform.Find("ItemTimer").GetComponent<Image>();
        cooldown = itemSprite.transform.Find("ImageCooldown").GetComponent<Image>();
    }

    public void Update()
    {
        if(!taken.Value) return;
        var player = transform.parent.GetComponent<ElympicsBehaviour>().PredictableFor;
        if(player != Elympics.Player) return;
        itemSprite.sprite = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        cooldown.sprite = itemSprite.sprite;
        cooldown.fillAmount = duration.Value / 10;
    }

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
            taken.Value = true;
            duration.Value -= Elympics.TickDuration;
            if(duration.Value < 0)
            {
                transform.SetParent(null);
                ElympicsDestroy(gameObject);
            }
        }
    }
}
