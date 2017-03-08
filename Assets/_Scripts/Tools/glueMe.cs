using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class glueMe : MonoBehaviour
{

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Wood")
        {
            //ContactPoint contact = collision.contacts[0];
            GameObject collidedObject = collider.gameObject;
            if (this.gameObject.transform.parent == null)
            {
                Rigidbody rigi = this.gameObject.GetComponent<Rigidbody>();
                if (rigi != null)
                {
                    Destroy(rigi);
                }
                this.gameObject.transform.rotation = collidedObject.transform.rotation;
				//this.gameObject.transform.position = contact.point - collision.transform.up * 0.01f; 
                //Rigidbody rigi = this.gameObject.GetComponent<Rigidbody>();
                //rigi.constraints = RigidbodyConstraints.FreezeAll;
                //rigi.freezeRotation = true;
                this.transform.SetParent(collidedObject.transform);
            }
            else if(collidedObject.transform.parent != transform.parent && collidedObject.transform != transform.parent)
            {
                Pickup pu = collidedObject.GetComponent<Pickup>();
                if (pu != null)
                    pu.GetReleased();

                collidedObject.transform.SetParent(this.gameObject.transform.parent);

                Rigidbody rbWood = collidedObject.GetComponent<Rigidbody>();
                if (rbWood != null)
                {
                    Destroy(rbWood);
                    //rbWood.constraints = RigidbodyConstraints.FreezeAll;
                    //rbWood.freezeRotation = true;
                }

            }
        }
    }

}
