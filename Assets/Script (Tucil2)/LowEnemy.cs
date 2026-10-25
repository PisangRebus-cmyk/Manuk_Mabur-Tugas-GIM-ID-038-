using UnityEngine;

public class LowEnemy : Enemy
{
    [SerializeField] private float turnTime = 2f;

    private float timer;
    private int direction = 1;

    protected override void Move()
    {
        timer += Time.fixedDeltaTime;
        if (timer >= turnTime)
        {
            direction = -direction;
            timer = 0f;
        }
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
    }
}