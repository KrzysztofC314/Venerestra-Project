using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedMovement : MonoBehaviour
{

    private float speed;
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float sprintSpeed;
    [SerializeField]
    private float crouchSpeed;
    [SerializeField]
    private float jumpForce;
    private float jumpSpeed;
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
    private bool cantStandUp;
    public bool ledgeDetected;
    [HideInInspector]
    public bool crouchAnim;
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
    [SerializeField]
    private AudioSource footsteps;

    private KeyCode key;

    [SerializeField] private Vector2 offset1;
    [SerializeField] private Vector2 offset2;
    private Vector2 climbBegunPosition;
    private Vector2 climbOverPosition;

    private bool canGrabLedge = true;
    private bool canClimb;
    [HideInInspector] public bool isClimbing;
    
    

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
        cantStandUp = Physics2D.OverlapCircle(ceillingCheck.position, checkRadius, whatIsGround);

        moveInput = Input.GetAxis("Horizontal");
        if (isGrounded)
        {
            velocityX = moveInput * speed;
            jumpSpeed = Jumpspeed(speed);
        }
        else
        {
            velocityX = moveInput * jumpSpeed;
        }
        rb.velocity = new Vector2(velocityX, rb.velocity.y);

        if(facingRight == false && moveInput > 0)
        {
            Flip();

        } else if(facingRight == true && moveInput < 0)
        {
            Flip();
        }

        if (Mathf.Abs(velocityX) > 0)
        {
            footsteps.enabled = true;
        }
        else
        {
            footsteps.enabled = false;
        }
    }

    void Update()
    {
        CheckForLedge();
        if (isGrounded == true)
        {
            extraJumps = extraJumpsValue;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && extraJumps > 0)
        {
            
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;

        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && extraJumps == 0 && isGrounded == true)
        {
            rb.velocity = Vector2.up * jumpForce;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            Crouch();
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            Sprint();
        }
        else
        {
            Default();
        }

    }

    private void CheckForLedge()
    {
        if (ledgeDetected && canGrabLedge)
        {
            canGrabLedge = false;

            Vector2 ledgePosition = GetComponentInChildren<LedgeDetection>().transform.position;
            climbBegunPosition = ledgePosition + offset1;
            climbOverPosition = ledgePosition + offset2;

            canClimb = true;
        }
        if (canClimb)
        {
            transform.position = climbBegunPosition;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                isClimbing = true;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                canClimb = false;
                transform.position = transform.position;
            }
        }
    }

    public void LedgeClimbOver()
    {
        isClimbing = false;
        canClimb = false;
        transform.position = climbOverPosition;
        canGrabLedge = true;
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
        offset1.x *= -1;
        offset2.x *= -1;
    }

    void Sprint()
    {
        if (!cantStandUp)
        {
            speed = sprintSpeed;
        }
    }

    void Crouch()
    {
        crouchingCollider.enabled = true;
        standingCollider.enabled = false;
        canGrabLedge = false;
        speed = crouchSpeed;
        crouchAnim = true;
    }

    void Default()
    {
        if (!cantStandUp)
        {
            standingCollider.enabled = true;
            crouchingCollider.enabled = false;
            speed = walkSpeed;
            crouchAnim = false;
            canGrabLedge = true;
        }
    }
    private float Jumpspeed(float speed)
    {
        jumpSpeed = speed;
        return jumpSpeed;
    }
   
}