using UnityEngine;

public class Trigger_Point : MonoBehaviour
{
    public Logic Logic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<Logic>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
    
            Logic.addScore(1);
        
    }
}
