using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateLobbyPlayer : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;

    void Update()
    {
        if(rb.velocity.x > 0.1 || rb.velocity.x < -0.1)
        {
            animator.SetBool("isWalking", true);
            animator.speed = Mathf.Abs(rb.velocity.x) * 0.2f;
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.speed = 1;
        }
    }
}
