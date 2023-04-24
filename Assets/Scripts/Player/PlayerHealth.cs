using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int setHealth;
    private int health;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    
    public void Hurt(int damage)
    {
        health -= damage;
        Debug.Log("Hit!");
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Dead");
    }
}
