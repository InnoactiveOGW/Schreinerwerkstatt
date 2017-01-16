using UnityEngine;
using System.Collections;

public class glueMe : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wood")
        {
			if (collision.contacts.Length > 0) {
				ContactPoint contact = collision.contacts [0];
			
                if (this.gameObject.transform.parent == null)
                {
                    //FixedJoint joint = this.gameObject.AddComponent<FixedJoint>();
                    this.gameObject.transform.rotation = collision.gameObject.transform.rotation;
					this.gameObject.transform.position = contact.point - collision.transform.up * 0.01f; 
                    Rigidbody rigi = this.gameObject.GetComponent<Rigidbody>();
                    rigi.constraints = RigidbodyConstraints.FreezeAll;
                    Rigidbody rbWood = collision.gameObject.GetComponent<Rigidbody>();
                    if(rbWood != null)
                    {
                        rbWood.constraints = RigidbodyConstraints.FreezeAll;
						rbWood.isKinematic = true;
						rbWood.useGravity = false;
                    }
                    //   rigi.isKinematic = true;
                    this.transform.SetParent(collision.gameObject.transform);
                }
                else
                {
                    Pickup pu = collision.gameObject.GetComponent<Pickup>();
                    if (pu != null)
                        pu.GetReleased();

                    collision.gameObject.transform.SetParent(this.gameObject.transform.parent);
                }

                // joint.connectedBody = collision.gameObject.GetComponent<Rigidbody>();

                return;
            }
		}
    }

}
