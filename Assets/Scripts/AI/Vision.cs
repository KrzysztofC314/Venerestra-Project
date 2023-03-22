using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float visionAngle;
    [SerializeField]
    private float visionRange;
    private string visionDirection;

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
        visionDirection = controller.facingDirection;
        Vector2 direction = player.position - transform.position;
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
            if (raycast.collider.CompareTag("Player")&& controller.state == AIController.State.Patrol)
            {
                Debug.DrawRay(visionPosition.position, direction, Color.blue);
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