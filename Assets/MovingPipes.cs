using JetBrains.Annotations;
using UnityEngine;

public class MovingPipes : MonoBehaviour
{
    private float baseSpeed = 1;
    public float moveSpeed = 1;
    public float deadZone = -25;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + (Vector3.left * baseSpeed * moveSpeed) * Time.deltaTime;

        if (transform.position.x < deadZone)
        {
            Debug.Log("Pipe Deleted");
            Destroy(gameObject);
        }
    }
}
