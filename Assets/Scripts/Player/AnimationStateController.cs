using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    // [SerializeField] private Jump jump;
    [SerializeField] private GameObject gun;
    void Start()
    {
        
    }

    // Update is called once per frame
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
        // if(jump == null) return;
        // if(jump.jumping) 
        // {
        //     animator.SetBool("isJumping", true);
        //     animator.SetBool("isWalking", false);
        // }
        // else
        // {
        //     animator.SetBool("isJumping", false);
        // }
        if(gun.activeSelf) animator.SetBool("hasGun", true);
        else animator.SetBool("hasGun", false);
    }

    public void GunAnimation(bool change)
    {
        animator.SetBool("hasGun", change);
    }
}
