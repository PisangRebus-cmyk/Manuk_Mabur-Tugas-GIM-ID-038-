using UnityEngine;
using UnityEngine.InputSystem;

public class SavePoint : MonoBehaviour
{
    private bool playerInRange;

    private void Update()
    {
        var kb = Keyboard.current;
        if (playerInRange && kb != null && kb.eKey.wasPressedThisFrame)
        {
            GameManager.Instance.SetCheckpoint(transform.position + Vector3.up * 0.5f);
            GameManager.Instance.SaveGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerInRange = false;
    }
}