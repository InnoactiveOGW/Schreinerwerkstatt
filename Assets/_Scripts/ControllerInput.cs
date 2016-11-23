using UnityEngine;
using System.Collections;

public class ControllerInput : MonoBehaviour
{

    public float pickupRadius = 1f;
    Pickup pickedObject;

    SteamVR_TrackedObject inputDevice;
    SteamVR_Controller.Device controller
    {
        get
        {
            return SteamVR_Controller.Input((int)inputDevice.index);
        }
    }

    void Start()
    {
        inputDevice = GetComponentInChildren<SteamVR_TrackedObject>();
    }

    void Update()
    {
        if (controller == null) {

        }

        if (controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger))
        {
            Collider[] pickups = Physics.OverlapSphere(transform.position, pickupRadius);
            if (pickups != null)
            {
                Collider nearestCollider = null;
                float minDis = pickupRadius;

                foreach (Collider collider in pickups)
                {
                    var currDis = Vector3.Distance(collider.transform.position, transform.position);
                    if (currDis < minDis) {
                        minDis = currDis;
                        nearestCollider = collider; 
                    }
                }

                if (nearestCollider) {
                    Pickup pickup = nearestCollider.GetComponent<Pickup>();
                    if (pickup != null)
                    {
                        pickup.GetPickedUp(gameObject);
                        pickup.transform.rotation = controller.transform.rot;

                        pickup.transform.localPosition = new Vector3(0, 0, 0);
                        // -> muss in das jeweilige Objekt ausgelagert werden, jedes Objekt muss selbst wissen wo sich der Ankerpunkt des Controllers befinden soll!

                        Rigidbody rb = pickup.GetComponent<Rigidbody>();
                        if (rb)
                        {
                            rb.constraints = RigidbodyConstraints.FreezeAll;
                        }
                        pickedObject = pickup;
                    }
                }

            }
        }

        if (controller.GetPressUp(Valve.VR.EVRButtonId.k_EButton_Grip))
        {
            if (pickedObject != null)
            {
                pickedObject.GetReleased(controller.velocity);
                Rigidbody rb = pickedObject.GetComponent<Rigidbody>();
                if (rb)
                {
                    rb.constraints = RigidbodyConstraints.None;
                }

                pickedObject = null;
            }
        }

    }
}
