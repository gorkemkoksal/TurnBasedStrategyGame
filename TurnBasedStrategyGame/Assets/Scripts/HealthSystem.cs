using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event Action OnDead;
    public event Action OnDamaged;

    [SerializeField] private int health = 100;
    private int _maxHealth;
    private void Awake()
    {
        _maxHealth = health;
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        if (health < 0) { health = 0; }
        OnDamaged?.Invoke();
        if (health == 0) { OnDead?.Invoke(); }
        print(health);
    }

    public float GetHealthNormalized()
    {
        return (float)health / _maxHealth;
    }

}
