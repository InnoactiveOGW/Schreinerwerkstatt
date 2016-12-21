using UnityEngine;
using System.Collections;

public class Glue : Pickup
{
    public GameObject glue;
    public float wait = 0;

    // Update is called once per frame
    void Update()
    {

        SteamVR_TrackedObject inputDevice = this.gameObject.GetComponentInParent<SteamVR_TrackedObject>();
        if(inputDevice != null) { 
            SteamVR_Controller.Device controller = SteamVR_Controller.Input((int)inputDevice.index);
            if (controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger) && Time.time - wait > 1)
            {
                GameObject thisglue = (GameObject)Instantiate(glue, this.transform.position + this.transform.forward * (float)0.2, Quaternion.identity);
                wait = Time.time;
            }
        }
    }
}
