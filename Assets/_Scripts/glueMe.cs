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
            foreach (ContactPoint contact in collision.contacts)
            {
                if (this.gameObject.transform.parent == null)
                {
                    //FixedJoint joint = this.gameObject.AddComponent<FixedJoint>();
                    this.gameObject.transform.rotation = collision.gameObject.transform.rotation;
                    Rigidbody rigi = this.gameObject.GetComponent<Rigidbody>();
                    rigi.constraints = RigidbodyConstraints.FreezeAll;
                    Rigidbody rbWood = collision.gameObject.GetComponent<Rigidbody>();
                    if(rbWood != null)
                    {
                        rbWood.constraints = RigidbodyConstraints.FreezeAll;
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
