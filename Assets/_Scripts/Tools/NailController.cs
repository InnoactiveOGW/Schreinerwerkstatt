using UnityEngine;
using System.Collections;

public class NailController : Pickup {

    public bool canBeNailed = false;
    public GameObject wood = null;
    private float oldMass = 0;

    void OnTriggerEnter(Collider collision) {
        if(collision.gameObject.tag == "Wood" && wood == null)
        {
            canBeNailed = true;
            wood = collision.gameObject;
            Rigidbody woodRB = wood.GetComponent<Rigidbody>();
            if (woodRB != null)
            {
                oldMass = woodRB.mass;
                woodRB.mass = 10000;
                woodRB.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (wood == collision.gameObject)
        {
            Debug.Log("nail lost contact with wood");
            Rigidbody woodRB = wood.GetComponent<Rigidbody>();
            if(woodRB != null) { 
                woodRB.mass = oldMass;
                woodRB.constraints = RigidbodyConstraints.None;
            }
            canBeNailed = false;
            wood = null;
        }
    }

    public void getPinnedToWood(float force)
    {
        wood.transform.SetParent(null);
        transform.position = transform.position - transform.up * force * 0.001f;     
               
        GetReleased();
        wood.transform.SetParent(this.gameObject.transform);
    }
}
