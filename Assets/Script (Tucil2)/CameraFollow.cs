using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 0.15f;
    [SerializeField] private Vector2 offset = new Vector2(0f, 1f);

    [Header("Level limits (optional)")]
    [SerializeField] private bool useLimits = false;
    [SerializeField] private Vector2 minPosition = new Vector2(-10f, -2f);
    [SerializeField] private Vector2 maxPosition = new Vector2(30f, 5f);

    private Vector3 velocity;

    private void Start()
    {
        // find the player automatically if the slot is empty
        if (target == null)
        {
            PlayerHealth player = FindAnyObjectByType<PlayerHealth>();
            if (player != null) target = player.transform;
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 goal = new Vector3(target.position.x + offset.x,
                                   target.position.y + offset.y,
                                   transform.position.z);   // keep the camera's Z

        if (useLimits)
        {
            goal.x = Mathf.Clamp(goal.x, minPosition.x, maxPosition.x);
            goal.y = Mathf.Clamp(goal.y, minPosition.y, maxPosition.y);
        }

        transform.position = Vector3.SmoothDamp(transform.position, goal, ref velocity, smoothTime);
    }
}