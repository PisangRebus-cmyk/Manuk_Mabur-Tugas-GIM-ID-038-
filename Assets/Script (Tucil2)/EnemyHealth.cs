using UnityEngine;

public class EnemyHealth : Health
{
    [SerializeField] private int scoreReward = 10;

    protected override void Die()
    {
        GameManager.Instance.AddScore(scoreReward);
        AudioManager.Play(Sfx.EnemyDie);
        Destroy(gameObject);
    }
}