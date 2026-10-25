using UnityEngine;

public class HealthPickup : Collectible
{
    [SerializeField] private int healAmount = 20;

    protected override bool OnCollected(GameObject player)
    {
        if (!player.TryGetComponent(out Health health)) return false;
        if (health.IsFull) return false;   // keep the potion if HP is full

        health.Heal(healAmount);
        AudioManager.Play(Sfx.Heal);
        return true;
    }
}