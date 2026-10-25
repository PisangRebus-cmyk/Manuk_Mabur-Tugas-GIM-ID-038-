using System;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    public int MaxHealth => maxHealth;
    public int CurrentHealth { get; private set; }
    public bool IsDead => CurrentHealth <= 0;
    public bool IsFull => CurrentHealth >= maxHealth;

    public event Action<int, int> Changed;   // (current, max)

    protected virtual void Awake()
    {
        CurrentHealth = maxHealth;
        Changed?.Invoke(CurrentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (IsDead) return;
        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);
        Changed?.Invoke(CurrentHealth, maxHealth);
        if (IsDead) Die();
    }

    public void Heal(int amount)
    {
        if (IsDead) return;
        CurrentHealth = Mathf.Min(CurrentHealth + amount, maxHealth);
        Changed?.Invoke(CurrentHealth, maxHealth);
    }

    public void SetCurrent(int value)
    {
        CurrentHealth = Mathf.Clamp(value, 1, maxHealth);
        Changed?.Invoke(CurrentHealth, maxHealth);
    }

    protected abstract void Die();
}