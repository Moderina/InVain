using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class Pistol : ElympicsMonoBehaviour, IUpdatable
{
    [SerializeField] private string bulletType;
    [SerializeField] private Transform bulletPoint;
    [SerializeField] private int speed = 5;
    [SerializeField] private float reloadTime = 0.2f;
    private bool lastInput = false;
    private float time = 0f;

    public void Shoot(Vector3 mouse, bool newInput)
    {
        if (!Elympics.IsServer || time > 0) return;
        if (newInput && !lastInput)
        {
            Vector2 force = new Vector2(mouse.x - bulletPoint.position.x, mouse.y - bulletPoint.position.y).normalized;
            var bullet = ElympicsInstantiate("bullets/force", ElympicsPlayer.World);
            bullet.transform.position = bulletPoint.position;
            bullet.GetComponent<Rigidbody2D>().AddForce(force * speed, ForceMode2D.Impulse);
            time = bullet.GetComponent<Bullet>().cooldown;
        }
        lastInput = newInput;

    }

    public void ElympicsUpdate()
    {
        time -= Elympics.TickDuration;
    }
}
