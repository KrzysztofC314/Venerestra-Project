using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour
{
    const string Left = "left";
    const string Right = "right";
    private float angle;
    

    public GameObject enemy;
    private AIController controller;
    [SerializeField]
    Transform visionPosition;
    [SerializeField]
    float visionAngle;
    [SerializeField]
    float visionRange;
    private string visionDirection;

    [SerializeField]
    private Transform player;


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
            if (raycast.collider.CompareTag("Player"))
            {
                print("located");
                Debug.DrawRay(visionPosition.position, direction, Color.blue);
                controller.state = AIController.State.Chase;
            }
        }
    }
}