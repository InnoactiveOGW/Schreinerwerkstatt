using UnityEngine;
using System.Collections;

public class Glue : Pickup
{



    public GameObject glue;
    public float wait = 0;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("g") && Time.time - wait > 1)
        {
            GameObject thisglue = (GameObject)Instantiate(glue, this.transform.position + this.transform.forward * (float)0.2, Quaternion.identity);
            wait = Time.time;
        }

    }
}
