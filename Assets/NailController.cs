using UnityEngine;
using System.Collections;

public class NailController : Pickup {

    public bool canBeNailed = false;
    public GameObject wood = null;

    void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Wood" && wood == null)
        {
            Debug.Log("nail can be used with hammer");
            canBeNailed = true;
            wood = collision.gameObject;
        }

    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Wood")
        {
            canBeNailed = false;
            wood = null;
        }
    }

    public void getPinnedToWood(float force)
    {
        transform.position = transform.position - transform.up * force;            
        this.gameObject.transform.parent = null;
        wood.transform.parent = this.gameObject.transform;
    }
}
