using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using System;

public class Pistol : ElympicsMonoBehaviour, IUpdatable
{
    public string bulletType;
    [SerializeField] private Transform bulletPoint;
    public int bulletammount = 5;
    [SerializeField] private int speed = 5;
    [SerializeField] private float reloadTime = 0.2f;
    private bool lastInput = false;
    private float time = 0f;
    private int bulletsShot = 0;
    private bool toDestroy = false;

    public void Shoot(Vector3 mouse, bool newInput)
    {
        if (!Elympics.IsServer || time > 0) return;
        if (bulletType == "dzban") return;
        if (newInput && !lastInput)
        {
            Vector2 force = new Vector2(mouse.x - bulletPoint.position.x, mouse.y - bulletPoint.position.y).normalized;
            var bullet = ElympicsInstantiate("bullets/"+bulletType, ElympicsPlayer.World);
            bullet.transform.position = bulletPoint.position;
            bullet.GetComponent<Rigidbody2D>().AddForce(force * speed, ForceMode2D.Impulse);
            //time = bullet.GetComponent<Bullet>().cooldown;
            time = reloadTime;
            if(++bulletsShot == bulletammount) 
            {
                toDestroy = true;
                bulletType = "dzban";
                bulletsShot = 0;
            }
        }
        lastInput = newInput;

    }


    public void ElympicsUpdate()
    {
        time -= Elympics.TickDuration;
        if(toDestroy) 
        {
            // ElympicsDestroy(gameObject);
            ElympicsDestroy(transform.parent.parent.GetChild(3).gameObject);
            toDestroy = false;
        }
    }
}
