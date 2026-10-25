using System.Collections.Generic;
using UnityEngine;

public class ChaserEnemy : Enemy
{
    [SerializeField] private float detectRange = 8f;
    [SerializeField] private float repathInterval = 0.4f;
    [SerializeField] private float waypointReach = 0.2f;

    private Transform target;
    private List<Vector2> path = new List<Vector2>();
    private int index;
    private float nextRepath;

    protected override void Awake()
    {
        base.Awake();
        rb.gravityScale = 0f;  
    }

    private void Start()
    {
        PlayerHealth player = FindAnyObjectByType<PlayerHealth>();
        if (player != null) target = player.transform;
    }

    protected override void Move()
    {
        if (target == null || Pathfinder.Instance == null ||
            Vector2.Distance(rb.position, target.position) > detectRange)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

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