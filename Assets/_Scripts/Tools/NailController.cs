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
                woodRB.constraints = RigidbodyConstraints.FreezeAll;
                woodRB.isKinematic = true;
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

    public virtual void getPinnedToWood(float force)
    {
        if (preventProgress)
            return;
        transform.position = transform.position - transform.up * force * 0.001f;
        foreach (GameObject wood in woods)
        {
            if(wood.transform.parent != parent)
                wood.transform.SetParent(parent);
        }
    }

    public virtual void getPinnedToWood(Vector3 velocity, Vector3 hammerPosition)
    {
        if (preventProgress)
            return;

        float force = velocity.magnitude;
        transform.position = transform.position - transform.up * force * 0.001f;
        foreach (GameObject wood in woods)
        {
            if (wood.transform.parent != parent)
                wood.transform.SetParent(parent);
        }
    }
}
