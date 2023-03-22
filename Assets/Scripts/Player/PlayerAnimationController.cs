using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator anim;
    [SerializeField]
    private AdvancedMovement movement;
    private float moveInput;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        moveInput = movement.velocityX;
        anim.SetFloat("moveInput", moveInput);
    }

    void Crouch(bool isCrouching)
    {
        anim.SetBool("isCrouching", isCrouching);
    }
    void Jump(bool isJumping)
    {
        anim.SetBool("isJumping", isJumping);
    }
}
