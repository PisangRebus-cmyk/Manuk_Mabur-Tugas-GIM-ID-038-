using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : Enemy
{
    [Header("Shooting")]
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float shootInterval = 1.5f;
    [SerializeField] private float shootRange = 12f;
    [SerializeField] private float spawnOffset = 1f;

    [Header("Chasing")]
    [SerializeField] private float detectRange = 14f;
    [SerializeField] private float stopDistance = 4f;     // keeps its distance from the player
    [SerializeField] private float repathInterval = 0.4f;
    [SerializeField] private float waypointReach = 0.25f;

    private Transform target;
    private List<Vector2> path = new List<Vector2>();
    private int index;
    private float nextRepath;
    private float nextShootTime;

    protected override void Awake()
    {
        base.Awake();
        rb.gravityScale = 0f;   // the boss floats
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
        if (target == null || Pathfinder.Instance == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float distance = Vector2.Distance(rb.position, target.position);

        // too far, or close enough: stand still (it can still shoot)
        if (distance > detectRange || distance <= stopDistance)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // recalculate the path a few times per second
        if (Time.time >= nextRepath)
        {
            path = Pathfinder.Instance.FindPath(rb.position, target.position);
            index = 0;
            nextRepath = Time.time + repathInterval;
        }

        if (index >= path.Count)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 waypoint = path[index];
        if (Vector2.Distance(rb.position, waypoint) < waypointReach)
        {
            index++;
            return;
        }

        rb.linearVelocity = (waypoint - rb.position).normalized * moveSpeed;
    }
}