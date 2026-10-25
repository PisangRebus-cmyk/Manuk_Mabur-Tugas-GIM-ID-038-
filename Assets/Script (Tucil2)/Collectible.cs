using UnityEngine;

public abstract class Collectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (OnCollected(other.gameObject))
            Destroy(gameObject);
    }

    protected abstract bool OnCollected(GameObject player);
}