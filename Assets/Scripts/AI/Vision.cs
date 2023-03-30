using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Vision : MonoBehaviour
{
    const string Left = "left";
    const string Right = "right";

    private float angle;
    [SerializeField]
    private GameObject enemy;
    private AIController controller;
    [SerializeField]
    private Transform visionPosition;
    [SerializeField]
    private Transform playerPosition;

    [SerializeField]
    private GameObject patrolLight;
    [SerializeField]
    private float patrolVisionAngle;
    [SerializeField]
    private float patrolVisionRange;

    [SerializeField]
    private GameObject chaseLight;
    [SerializeField]
    private float chaseVisionAngle;
    [SerializeField]
    private float chaseVisionRange;
    private string visionDirection;
    private float visionAngle;
    private float visionRange;

    [SerializeField]
    private Transform player;
    [SerializeField]
    private float stunWait;

    void Start()
    {
        controller = enemy.GetComponent<AIController>();
        
    }

    private void FixedUpdate()
    {
        switch (controller.state)
        {
            default:
            case AIController.State.Patrol:
                visionAngle = patrolVisionAngle;
                visionRange = patrolVisionRange;
                patrolLight.SetActive(true);
                chaseLight.SetActive(false);
                break;
            case AIController.State.Chase:
                visionAngle = chaseVisionAngle;
                visionRange = chaseVisionRange;
                patrolLight.SetActive(false);
                chaseLight.SetActive(true);
                break;
        }
        visionDirection = controller.facingDirection;
        Vector2 direction = playerPosition.position - transform.position;
        if (visionDirection == Right)
        {
            angle = Vector3.Angle(direction, visionPosition.right);
        }
        else if(visionDirection == Left)
        {
            angle = Vector3.Angle(direction, -visionPosition.right);
        }
        RaycastHit2D raycast = Physics2D.Raycast(visionPosition.position, direction, visionRange);
        if(angle < visionAngle / 2)
        {
            Debug.DrawRay(visionPosition.position, direction, Color.blue);
            if (raycast.collider.CompareTag("Player")&& controller.state == AIController.State.Patrol)
            {
                controller.speed = 0;
                Invoke("Located", stunWait);
            }
        }
    }
    private void Located()
    {
        print("located");
        controller.state = AIController.State.Chase;
    }
}