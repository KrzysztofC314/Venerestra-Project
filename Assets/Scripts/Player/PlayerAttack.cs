using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform hitCheck;
    [SerializeField] Vector2 hitCheckSize;
    
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.X))
        {
            Kill();
        }
    }
    
    void Kill()
    {
        var hit = Physics2D.BoxCast(hitCheck.position, hitCheckSize, 0f, Vector2.zero, LayerMask.NameToLayer("Enemy"));
        if (hit.collider != null)
        {
            var enemy = hit.collider.GetComponent<AIController>();
            enemy.Die();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(hitCheck.position, new Vector3(hitCheckSize.x, hitCheckSize.y, 1));
    }
}
