using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float moveSpeed = 14f;


    private float moveInput;
    private bool isGrounded;
    [SerializeField] private LayerMask canJump;

    private bool canDash = true;
    private bool isDashing;
    [SerializeField] private float dashForce = 50f;
    [SerializeField] private float dashTime = 0.1f;
    [SerializeField] private float dashCooldown = 1f;

    public float knockbackForce;
    public float knockbackCounter;
    public float knockbackTotalTime;
    public bool knockbackFromRight;
    private enum MovementState { idle,running,jumping,falling}

    private Animator anim;
    [SerializeField] private Rigidbody2D rb;
    private SpriteRenderer sr;
    private BoxCollider2D coll;
    private TrailRenderer tr;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        tr = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        MovementUpdate();
    }
    private void MovementUpdate()
    {
        //Xac dinh neu doi tuong dung tren mat dat 
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.1f);
        if (isDashing)
        {
            return;
        }
        //Di chuyen sang trai phai
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);    
        //Nhay  
        if (Input.GetButtonDown("Jump") && IsGround())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        AnimationMovementUpdate();
    }
     void AnimationMovementUpdate()
    {
        
        MovementState state;
        if (isDashing)
        {
            return;
        }
   
        if (moveInput < 0f)
        {
            sr.flipX = true;
            state = MovementState.running;   
        }
        else if (moveInput > 0f)
        {
            sr.flipX = false;
            state = MovementState.running;
        }
        else
        {
            state = MovementState.idle;
        }
        if (rb.velocity.y < -0.1f)
        {
            state = MovementState.falling;
        }
        else if (rb.velocity.y > 0.1f)
        {
            state = MovementState.jumping;
        }
        
        anim.SetInteger("state",(int)state);
    }
    private bool IsGround()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, canJump);
    }


    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true; 
        float originalGravity = rb.gravityScale;
        //rb.gravityScale = 0f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(transform.localScale.x, 0f), dashForce * dashTime, canJump);

        // Nếu có va chạm
        if (hit.collider != null)
        {
            // Di chuyển người chơi đến vị trí gần nhất với va chạm
            rb.position = hit.point;
        }
        float moveDirection = Mathf.Sign(rb.velocity.x);
        float dashDirection = Mathf.Clamp(moveDirection, -1f, 1f);
        rb.velocity = new Vector2(dashDirection * dashForce, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashTime);
        tr.emitting = false;
        //rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

}
