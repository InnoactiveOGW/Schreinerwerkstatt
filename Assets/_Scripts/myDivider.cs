using UnityEngine;
using System.Collections;

public class myDivider : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }


    void OnTriggerEnter(Collider other)
    {
        //   Destroy(this.gameObject);
        if (other.gameObject.tag == "saw")
        {
            // myObject.transform.position = new Vector3(0, (float)2.22, -2);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z / 2);
        }

    }
}
