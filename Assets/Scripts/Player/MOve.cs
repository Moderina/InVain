using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class MOve : ElympicsMonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;

    [SerializeField] private GameObject sprite;
    [SerializeField] private Transform head;
    private int faceDir = 1;
    [SerializeField] private float jumpPower;
    public void Movement(int direction, int jump, Vector3 mousePos)
    {
        rb.velocity = new Vector2(direction * speed, 0);
        rb.AddForce(new Vector2(0, jump * jumpPower), ForceMode2D.Impulse);
        FaceDirection(mousePos);
    }

    public void FaceDirection(Vector3 mousePos) 
    {
        var headpos = head.position;
        var direction = (mousePos - headpos).normalized;
        if (faceDir > 0) 
        {
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            head.eulerAngles = new Vector3(0, 0, angle + 90);
            if ((angle > 90 || angle < -90))
            {
                faceDir = -faceDir;
                sprite.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else 
        {
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 180;
            head.eulerAngles = new Vector3(0, 0, angle - 90);
            if ( angle < -90 && angle > -270)
            {
                faceDir = -faceDir;
                sprite.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    void OnDrawGizmos() 
    {
        var mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Gizmos.DrawLine(head.position, mousepos);
    }
}
