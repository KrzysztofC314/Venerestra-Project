using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedMovement : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    private float moveInput;

    [SerializeField]
    private Collider2D standingCollider;
    [SerializeField]
    private Collider2D crouchingCollider;

    private Rigidbody2D rb;

    private bool facingRight = true;

    private bool isCrouching;
    private bool isGrounded;
    private bool canStandUp;
    public Transform groundCheck;
    public Transform ceillingCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    private int extraJumps;
    public int extraJumpsValue;
    
    //private Animator anim;

    void Start()
    {
        //anim = GetComponent<Animator>();
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
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if(facingRight == false && moveInput > 0)
        {
            Flip();

        } else if(facingRight == true && moveInput < 0)
        {
            Flip();
        }
        //if (moveInput == 0) {
        //   anim.SetBool("isRunning", false);
        //}
        //else {
        //     anim.SetBool("isRunning", true);
        //}
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
           //anim.SetBool("isJumping", true);

        } 
        else if(Input.GetKeyDown(KeyCode.UpArrow) && extraJumps == 0 && isGrounded == true)
        {
           rb.velocity = Vector2.up * jumpForce;
           //anim.SetBool("isJumping", false);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            isCrouching = true;
            crouchingCollider.enabled = true;
            standingCollider.enabled = false;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            isCrouching = false;
        }
        if (isCrouching == false && canStandUp == false)
        {
            standingCollider.enabled = true;
            crouchingCollider.enabled = false;
        }
       
        //if (isGrounded == false)
        //{
        //   anim.SetBool("isJumping", true);
        //}
        
        //else 
        //{
        //   anim.SetBool("isJumping", false);
        //}
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
   
}