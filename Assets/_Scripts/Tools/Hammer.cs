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
            //return controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);
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

                // Problem: rigidbody wird nicht direkt benutzt um Position des Objekts zu verändern => entweder Geschwindigkeit 
                // selbst berechnen oder Rigidbody verwenden (kann zu sehr merkwürdigen Ergebnissen führen!)
                float force = 1;
                Rigidbody thisRB = gameObject.GetComponent<Rigidbody>();
                if (thisRB)
                    force = thisRB.velocity.magnitude;
                else {
                    SteamVR_Controller.Device controller = getController();
                    if(controller != null)
                        force = controller.velocity.magnitude;

                    HapticFeedbackController hfc = getHapticFeedbackController();
                    if (hfc != null) {
                        // TODO

                    }
                }
                force = force > 0 ? force * 10 : 1;
                Debug.Log("force: " + force);
                nc.getPinnedToWood(force);
                // TODO Verformung des Nagels bei schlechtem Treffen -> Vektoren zum bewerten: hammer.up, nail.up -> sollten im rechten Winkel zueinander stehen
            }
        }
    }
}
