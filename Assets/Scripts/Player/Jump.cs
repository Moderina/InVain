using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class Jump : ElympicsMonoBehaviour
{
    private const float GroundCheckOffset = 0.01f;

    [Header("References")]
    [SerializeField] private Rigidbody2D playerRigidBody;
    [SerializeField] private BoxCollider2D playerCollider;
    [Header("Jump Variables")]
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float defaultGravity;
    [SerializeField] private float lowJumpMultiplier;
    [SerializeField] private float highJumpMultiplier;
    [SerializeField] private float fallingMultiplier;
    [SerializeField] private LayerMask groundMask;

    [Header("Quality of life")]
    [SerializeField] private float coyoteTime;
    private float coyoteTimeCounter;
    [SerializeField] private float jumpBufferDuration;
    private float jumpBufferCounter;
    private int rememberedJumpInput = 0;
    private bool rememberedGround = false;

    // For effects
    public bool jumping = false;
    private bool isMidAir = false;
    public event System.Action OnJumped;
    public event System.Action OnLanded;

    public void OnJumpInput(int jumpInput)
    {
        //We only jump when the jump button was pressed, not held
        bool startJump = false;
        if (rememberedJumpInput == 0 && jumpInput != 0) startJump = true;
        rememberedJumpInput = jumpInput;

        //Jump buffering
        if (startJump) jumpBufferCounter = jumpBufferDuration;
        //Checking if we're grounded + coyote time

        //if touching grass
        if (!jumping)
        //if (true)
        {
            coyoteTimeCounter = coyoteTime;

            if (isMidAir)
            {
                isMidAir = false;
                // if wasnt touching grass a second ago, he landed
                if (!rememberedGround)
                {
                    rememberedGround = true;
                    OnLanded?.Invoke();
                }
            }
        }

        float gravityMultiplier;
        //if can jump
        if (coyoteTimeCounter > 0 && jumpBufferCounter > 0)
        {
            //Actual jump
            playerRigidBody.velocity = Vector2.up * jumpVelocity;
            gravityMultiplier = 0;
            jumpBufferCounter = 0;
            coyoteTimeCounter = 0;

            isMidAir = true;
            OnJumped?.Invoke();
        }
        else if (playerRigidBody.velocity.y < 0)
        {
            //Faster falling
            gravityMultiplier = fallingMultiplier;
        }
        else if (playerRigidBody.velocity.y > 0 && jumpInput == 0)
        {
            //Low jump
            gravityMultiplier = lowJumpMultiplier;
        }
        else
        {
            //High jump
            gravityMultiplier = highJumpMultiplier;
        }

        playerRigidBody.velocity += gravityMultiplier * defaultGravity * Elympics.TickDuration * Vector2.up;
        //playerRigidBody.velocity += new Vector2(1, gravityMultiplier * defaultGravity * Elympics.TickDuration);

        coyoteTimeCounter -= Elympics.TickDuration;
        jumpBufferCounter -= Elympics.TickDuration;
    }

    public bool IsGrounded()
    {
        Debug.Log("check");
        Collider2D[] hits = Physics2D.OverlapBoxAll(playerRigidBody.position + GroundCheckOffset * Vector2.down, PlayerSizeScaled, 0, groundMask);
        foreach (var hit in hits)
        {
            Debug.Log(hit.name);
            if (hit.tag == "Ground")
            {
                return true;
            }
            return false;
        }
        return false;
    }

    private Vector2 PlayerSizeScaled => Vector2.Scale(transform.localScale, playerCollider.size);

    public void OnCollisionEnter2D(Collision2D col) 
    {
        List<ContactPoint2D> contacts = new List<ContactPoint2D>();
        if (playerCollider.GetContacts(contacts) == 0) return;
        if (col.gameObject.CompareTag("Ground")) 
        {
			jumping = false; 
		}
    }
    public void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground")) 
        {
			jumping = true; 
		}
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(1,2,0), new Vector3(1,0,0));
        Gizmos.DrawLine(playerRigidBody.position, playerRigidBody.position + GroundCheckOffset * Vector2.down * 10);
    }
}
