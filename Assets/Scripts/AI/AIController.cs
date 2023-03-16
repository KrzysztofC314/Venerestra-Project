using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    const string Left = "left";
    const string Right = "right";
    
    [HideInInspector]
    public string facingDirection;
    [HideInInspector]
    public Vector3 baseScale;
    private void Awake()
    {
        
    }

    private void Start()
    {
        facingDirection = Right;
        baseScale = transform.localScale;
    }
    // Update is called once per frame
    public void Update()
    {
       
    }

    public void ChangeDirection(string newDirection)
    {
        Vector3 newScale = baseScale;

        if (newDirection == Left)
        {
            newScale.x = -baseScale.x;
        }
        else if (newDirection == Right)
        {
            newScale.x = baseScale.x;
        }

        transform.localScale = newScale;

        facingDirection = newDirection;
    }
}
