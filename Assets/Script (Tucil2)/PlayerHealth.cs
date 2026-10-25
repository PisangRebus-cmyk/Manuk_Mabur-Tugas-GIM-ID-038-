using UnityEngine;

public class PlayerHealth : Health
{
    protected override void Die()
    {
        GameManager.Instance.OnPlayerDied();
    }
}