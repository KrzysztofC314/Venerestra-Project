using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedMovement : MonoBehaviour
{
    
    private float speed;
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float crouchSpeed;
    [SerializeField]
    private float jumpForce;
    private float moveInput;

    [SerializeField]
    private Collider2D standingCollider;
    [SerializeField]
    private Collider2D crouchingCollider;

    private Rigidbody2D rb;
    [HideInInspector]
    public float velocityX;

    private bool facingRight = true;

    private bool isCrouching;
    private bool isGrounded;
    private bool canStandUp;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private Transform ceillingCheck;
    [SerializeField]
    private float checkRadius;
    [SerializeField]
    private LayerMask whatIsGround;

    private int extraJumps;
    [SerializeField]
    private int extraJumpsValue;
    
    

    void Start()
    {
        speed = walkSpeed;
        extraJumps = extraJumpsValue;
        rb = GetComponent<Rigidbody2D>();
        standingCollider.enabled = true;
        crouchingCollider.enabled = false;

    }
   
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        canStandUp = Physics2D.OverlapCircle(ceillingCheck.position, checkRadius, whatIsGround);

        moveInput = Input.GetAxis("Horizontal");
        velocityX = moveInput * speed;
        rb.velocity = new Vector2(velocityX, rb.velocity.y);

        if(facingRight == false && moveInput > 0)
        {
            Flip();

        } else if(facingRight == true && moveInput < 0)
        {
            Flip();
        }
    }

    void Update()
    {
        if (isGrounded == true)
        {
           extraJumps = extraJumpsValue;
        }
        
        if(Input.GetKeyDown(KeyCode.UpArrow) && extraJumps > 0)
        {
           rb.velocity = Vector2.up * jumpForce;
           extraJumps--;

        } 
        else if(Input.GetKeyDown(KeyCode.UpArrow) && extraJumps == 0 && isGrounded == true)
        {
           rb.velocity = Vector2.up * jumpForce;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            isCrouching = true;
            crouchingCollider.enabled = true;
            standingCollider.enabled = false;
            speed = crouchSpeed;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            isCrouching = false;
        }
        if (isCrouching == false && canStandUp == false)
        {
            standingCollider.enabled = true;
            crouchingCollider.enabled = false;
            speed = walkSpeed;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
   
}