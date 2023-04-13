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
    private ChaseMode chaseMode;
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
    private float chillTimer = 0;
    [SerializeField]
    private float chillTime;
    private float shootTimer = 0;
    [SerializeField]
    private float shootTime;
    private bool noticed;

    void Start()
    {
        controller = enemy.GetComponent<AIController>();
        chaseMode = enemy.GetComponent<ChaseMode>();
    }

    private void Update()
    {
        if (controller.state == AIController.State.Chase)
        {
            if (!noticed)
            {
                shootTimer = 0;
                chillTimer += Time.deltaTime;
                if (chillTimer >= chillTime)
                {
                    controller.state = AIController.State.Patrol;
                }
            }
            else if (noticed)
            {
                chillTimer = 0;
                shootTimer += Time.deltaTime;
                if (shootTimer >= shootTime)
                {
                    chaseMode.Shoot();
                    shootTimer = 0;
                }
            }
        }
        else
        {
            chillTimer = 0;
            shootTimer = 0;
        }
    }

    private void FixedUpdate()
    {
        visionDirection = controller.facingDirection;
        Vector2 direction = playerPosition.position - transform.position;
        if (visionDirection == Right)
        {
            angle = Vector3.Angle(direction, visionPosition.right);
        }
        else if (visionDirection == Left)
        {
            angle = Vector3.Angle(direction, -visionPosition.right);
        }
        RaycastHit2D raycast = Physics2D.Raycast(visionPosition.position, direction, visionRange);
        switch (controller.state)
        {
            default:
            case AIController.State.Patrol:
                visionAngle = patrolVisionAngle;
                visionRange = patrolVisionRange;
                patrolLight.SetActive(true);
                chaseLight.SetActive(false);
                if (angle < visionAngle / 2)
                {
                    Debug.DrawRay(visionPosition.position, direction, Color.blue);
                    if (raycast.collider.CompareTag("Player"))
                    {
                        controller.speed = 0;
                        Invoke("Located", stunWait);
                    }
                }
                break;
            case AIController.State.Chase:
                visionAngle = chaseVisionAngle;
                visionRange = chaseVisionRange;
                patrolLight.SetActive(false);
                chaseLight.SetActive(true);
                if (angle < visionAngle / 2)
                {
                    Debug.DrawRay(visionPosition.position, direction, Color.blue);
                    if (raycast.collider.CompareTag("Player"))
                    {
                        noticed = true;
                    }
                    else
                    {
                        noticed = false;
                    }
                }
                else
                {
                    noticed = false;
                }
                break;
        }
    }
    private void Located()
    {
        print("located");
        controller.state = AIController.State.Chase;
    }
}