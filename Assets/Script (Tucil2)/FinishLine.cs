using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private bool requireBossDefeated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (requireBossDefeated && FindAnyObjectByType<BossEnemy>() != null)
        {
            Debug.Log("Defeat the boss first!");
            return;
        }

        GameManager.Instance.WinGame();
    }
}