using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;
    private Animator anim;
    private SpriteRenderer renderer;

    public float speed, jumpForece;
    public Transform groundCheck;
    public LayerMask ground;

    public bool isGround, isJump, moveLock;

    float moveLockSpeed,moveLockTime, moveLockStartTime;

    bool jumpPressed;
    public int jumpCount = 2;
    public float hMove;

    public Vector2 groundCheckSize;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(groundCheck.position, groundCheckSize);
        Gizmos.color = Color.green;
    }

    private void FixedUpdate()
    {
        if(!isGround)
            isGround = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, ground);

        GroundMovement();
        Jump();
    }

    void GroundMovement()
    {
        if (!moveLock)
        {
            rb.velocity = new Vector2(hMove * speed, rb.velocity.y);
        }
        else
        {
            if (Time.time - moveLockStartTime > moveLockTime)
            {
                moveLock = false;
                renderer.color = new Color(1, 1, 1);
                return;
            }
            hMove = moveLockSpeed/Mathf.Abs(moveLockSpeed);
            rb.velocity = new Vector2(moveLockSpeed, rb.velocity.y);

        }




        //发生移动事件
        if (hMove != 0)
        {
            transform.localScale = new Vector3(hMove, 1, 1);
            if(anim)
                anim.SetBool("Run", true);
        }
        else
        {
            if(anim)
                anim.SetBool("Run", false);
        }
    }

    public void moveLockEvent(Vector2 v, float time, bool beAttack)
    {
        moveLock = true;
        moveLockSpeed = v.x;
        rb.velocity = v;
        moveLockTime = time;
        moveLockStartTime = Time.time;
        if(beAttack)
            renderer.color = new Color(1, 0, 0);
    }
    public void JumpEvent()
    {
        if (jumpCount > 0)
        {
            jumpPressed = true;
        }
    }
    public void Jump()
    {
        if (isGround)
        {
            jumpCount = 2;
            isJump = false;
        }
        if (jumpPressed && isGround)
        {
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForece);
            jumpCount--;
            jumpPressed = false;
            isGround = false;
        }
        else if (jumpPressed && jumpCount > 0 && isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForece);
            jumpCount--;
            jumpPressed = false;
        }
    }
}
