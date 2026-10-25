using UnityEngine;

public class BossEnemy : Enemy
{
    [Header("Shooting")]
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float shootInterval = 1.5f;
    [SerializeField] private float shootRange = 12f;
    [SerializeField] private float spawnOffset = 1f;

    [Header("Hovering (optional)")]
    [SerializeField] private float hoverHeight = 0.5f;
    [SerializeField] private float hoverSpeed = 1.5f;

    private Transform target;
    private float nextShootTime;
    private Vector2 startPos;

    protected override void Awake()
    {
        base.Awake();
        rb.gravityScale = 0f;      
        startPos = rb.position;
    }

    private void Start()
    {
        PlayerHealth player = FindAnyObjectByType<PlayerHealth>();
        if (player != null) target = player.transform;
        nextShootTime = Time.time + shootInterval;
    }

    private void Update()
    {
        if (target == null || bulletPrefab == null) return;
        if (Vector2.Distance(transform.position, target.position) > shootRange) return;

        if (Time.time >= nextShootTime)
        {
            Shoot();
            nextShootTime = Time.time + shootInterval;
        }
    }

    private void Shoot()
    {
        Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
        Vector2 spawnPos = (Vector2)transform.position + direction * spawnOffset;

        Bullet bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        bullet.Launch(direction, gameObject.tag);   // owner = "Enemy"
        AudioManager.Play(Sfx.Shoot);
    }

    protected override void Move()
    {
        float y = startPos.y + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        rb.MovePosition(new Vector2(startPos.x, y));
    }
}