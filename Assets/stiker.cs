using UnityEngine;

public class stiker : MonoBehaviour
{
    public GameObject[] items;          // drag the items here (they can start inactive)
    public sekrip logic;          // the script that holds Manuk_idup

    void Start()
    {
        foreach (GameObject item in items)
            item.SetActive(false);      // initial condition: hidden
        
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<sekrip>();
    }

    void Update()
    {
        if (logic.Manuk_Idup = false)          // player died
        {
            foreach (GameObject item in items)
                item.SetActive(true);
        }
    }
}