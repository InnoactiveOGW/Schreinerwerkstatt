using UnityEngine;
using System.Collections;

public class Hammer : MonoBehaviour {

    SteamVR_Controller.Device getController()
    {
        SteamVR_TrackedObject inputDevice = this.gameObject.GetComponentInParent<SteamVR_TrackedObject>();
        if (inputDevice != null)
        {
            SteamVR_Controller.Device controller = SteamVR_Controller.Input((int)inputDevice.index);
            return controller;
        }
        return null;
    }

    HapticFeedbackController getHapticFeedbackController() {
        return GetComponentInParent<HapticFeedbackController>();
    }

    void OnTriggerEnter(Collider collider) {
        NailController nc = collider.gameObject.GetComponent < NailController >();
        
        if (nc != null && nc.canBeNailed)
        {
            Rigidbody rb = nc.gameObject.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
                Vector3 velocity = new Vector3(0,1,0);
                SteamVR_Controller.Device controller = getController();
                if(controller != null)
                    velocity = controller.velocity;
                HapticFeedbackController hfc = getHapticFeedbackController();
                if (hfc != null) {
                    // TODO: haptic feedback
                }
                nc.getPinnedToWood(velocity, transform.position);
            }
        }
    }
}
