using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator anim;
    [SerializeField]
    private AdvancedMovement movement;
    private float moveInput;
    private bool isCrouching;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("isCrouching", false);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        moveInput = Mathf.Abs(movement.velocityX);
        anim.SetFloat("moveInput", moveInput);
    }
    private void Update()
    {
        isCrouching = movement.crouchAnim;
        if (isCrouching)
        {
            Crouch(true);
        }
        if (!isCrouching)
        {
            Crouch(false);
        }
    }

    public void Crouch(bool isCrouching)
    {
        anim.SetBool("isCrouching", isCrouching);
    }
    public void Jump(bool isJumping)
    {
        anim.SetBool("isJumping", isJumping);
    }
}
