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
    private bool isClimbing;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        
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
        isClimbing = movement.canClimb;
        anim.SetBool("isCrouching", isCrouching);
        anim.SetBool("isClimbing", isClimbing);
    }

    private void LedgeClimb()
    {
        movement.LedgeClimbOver();
    }

}
