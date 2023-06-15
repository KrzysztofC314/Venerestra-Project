using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int setHealth;
    [HideInInspector] public int health;
    [SerializeField]
    private HealthBar healthBar;
    void Start()
    {
        health = setHealth;
        healthBar.SetMaxHealth(health);
    }

    void Update()
    {
        
    }
    
    public void Hurt(int damage)
    {
        health -= damage;
        healthBar.SetHealth(health);
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
