using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControllerInput : MonoBehaviour
{

    public float pickupRadius = 1f;
    Pickup pickedObject;

    HapticFeedbackController hfc;
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
        hfc = GetComponentInParent<HapticFeedbackController>();
    }

    void Update()
    {
        if (pickedObject == null && controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger))
        {
           
            

            Collider[] pickups = Physics.OverlapSphere(transform.position, pickupRadius);
            if (pickups != null)
            {
                Collider nearestCollider = null;
                float minDis = pickupRadius;

                foreach (Collider collider in pickups)
                {
                    var currDis = Vector3.Distance(collider.transform.position, transform.position);
                    if (currDis < minDis)
                    {
                        minDis = currDis;
                        nearestCollider = collider;
                    }
                }

                if (nearestCollider)
                {
                    WoodCreator wc = nearestCollider.GetComponent<WoodCreator>();
                    if (wc != null) {
                        wc.createNewWood();
                        Renderer wcRend =  wc.gameObject.GetComponent<Renderer>();
                        Material wcMat = wcRend.material;
                        wcMat.color = new Color(Random.value, Random.value, Random.value);
                        return;
                    }

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

                        //TODO - feedback for user -> vibration?
                        //controller.TriggerHapticPulse(2000);
                        
                        // SteamVR_Controller.Input((int)inputDevice.index).TriggerHapticPulse(2000);
                    }
                }
            }
        }

        if (pickedObject != null && controller.GetPressUp(Valve.VR.EVRButtonId.k_EButton_Grip))
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

    void hapticFeedback() {
        // TODO: setup values for vibration
        if (hfc)
            hfc.StartHapticVibration(controller, 0.1f, 0.1f);
    }
}
