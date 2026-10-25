using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifetime = 2f;

    private Rigidbody2D rb;
    private string ownerTag;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, string ownerTag)
    {
        this.ownerTag = ownerTag;
        rb.linearVelocity = direction.normalized * speed;
        Destroy(gameObject, lifetime);   // cleanup if it hits nothing
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger) return;                                    // ignore coins, potions
        if (ownerTag != null && other.CompareTag(ownerTag)) return;     // don't hit the shooter

        if (other.TryGetComponent(out Health health))
            health.TakeDamage(damage);

        Destroy(gameObject);   // hit a wall, floor or target
    }
}