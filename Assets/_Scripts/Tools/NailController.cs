using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NailController : MonoBehaviour {

    public Transform parent;
    public bool canBeNailed = false;
    public bool preventProgress = false;
    public List<GameObject> woods = null;
    private float oldMass = 0;

    void Start() {
        woods = new List<GameObject>();
    }

    void OnTriggerEnter(Collider collision) {
        if(collision.gameObject.tag == "Wood" && !preventProgress)
        {
            woods.Add(collision.gameObject);
            canBeNailed = true;
            Rigidbody woodRB = collision.gameObject.GetComponent<Rigidbody>();
            if (woodRB != null)
            {
                // oldMass = woodRB.mass;
                // woodRB.mass = 1000;
                woodRB.constraints = RigidbodyConstraints.FreezeAll;
                woodRB.isKinematic = true;
                //woodRB.constraints = RigidbodyConstraints.FreezePosition;
                //woodRB.constraints = RigidbodyConstraints.FreezeRotationY;
                //woodRB.constraints = RigidbodyConstraints.FreezeRotationZ;
            }
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (woods.Contains(collision.gameObject))
        {
            Debug.Log("nail lost contact with wood");
            Rigidbody woodRB = collision.gameObject.GetComponent<Rigidbody>();
            if(woodRB != null) { 
                woodRB.mass = oldMass;
                woodRB.constraints = RigidbodyConstraints.None;
                woodRB.isKinematic = false;
            }
            woods.Remove(collision.gameObject);
            if (woods.Count == 0)
                canBeNailed = false;
        }
    }

    public void getPinnedToWood(float force)
    {
        if (preventProgress)
            return;

        transform.position = transform.position - transform.up * force * 0.001f;

        //Debug.Log("pinning nail to wood");
        //foreach (GameObject wood in woods) {
        //    wood.transform.SetParent(null);
        //}
        //// old: transform.position = transform.position - transform.up * force * 0.001f;
        ////Pickup parentPickup = parent.GetComponent<Pickup>();
        ////parentPickup.GetReleased();

        // TODO

        foreach (GameObject wood in woods)
        {
            if(wood.transform.parent != parent)
                wood.transform.SetParent(parent);
        }
    }
}
