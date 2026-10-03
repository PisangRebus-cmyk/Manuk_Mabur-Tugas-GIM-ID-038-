using UnityEngine;

public class sekrip : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public float Kepakan;
    public CircleCollider2D mycirclecollider;
    public Logic Logic;
    public bool Manuk_Idup = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<Logic>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) == true && Manuk_Idup == true)
        {
            myRigidbody.linearVelocity = Vector2.up * Kepakan;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Logic.gameOver();
        Manuk_Idup = false;
    }
}
