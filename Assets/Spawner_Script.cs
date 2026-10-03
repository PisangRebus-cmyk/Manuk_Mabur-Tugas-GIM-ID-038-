using System;
using UnityEngine;

public class Spawner_Script : MonoBehaviour
{
    public GameObject pipe;
    public float spawnrate = 2;
    private float timer = 0;
    public float heightOffset = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnpipe();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < spawnrate)
        {
            timer += Time.deltaTime;
        } 
        else
        {
            spawnpipe();
            timer = 0;
        }
        
    }

    void spawnpipe()
    {
        float lowestPoint = transform.position.y - heightOffset;
        float highestPoint = transform.position.y + heightOffset;

        Instantiate(pipe, new Vector3(transform.position.x, UnityEngine.Random.Range(lowestPoint, highestPoint), 0), transform.rotation);
    }
}
