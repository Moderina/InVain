using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class Pistol : ElympicsMonoBehaviour
{
    [SerializeField] private string bulletType;
    [SerializeField] private Transform bulletPoint;
    [SerializeField] private int speed = 5;

    public void Shoot(Vector3 mouse)
    {
        if (!Elympics.IsServer) return;
        Vector2 force = new Vector2(mouse.x - bulletPoint.position.x, mouse.y - bulletPoint.position.y);
        var bullet = ElympicsInstantiate("bullets/force", ElympicsPlayer.World);
        bullet.transform.position = bulletPoint.position;
        bullet.GetComponent<Rigidbody2D>().AddForce(force * 5, ForceMode2D.Impulse);
    }
}
