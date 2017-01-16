using UnityEngine;
using System.Collections;

public class Glue : Pickup
{
    public GameObject glue;
    public float wait = 0;

    private void Start()
    {
        isPickedup = false;
    }
    // Update is called once per frame
    void Update()
    {

		bool triggerButton = getTriggerButton();

		if ((Input.GetKey("g")||triggerButton) && Time.time - wait > 1 && isPickedup)
        {
            GameObject thisglue = (GameObject)Instantiate(glue, this.transform.position + this.transform.forward * (float)0.2, Quaternion.identity);
            wait = Time.time;
        }
    }
	bool getTriggerButton() {
		SteamVR_TrackedObject inputDevice = this.gameObject.GetComponentInParent<SteamVR_TrackedObject> ();
		if (inputDevice != null && isPickedup) {
			SteamVR_Controller.Device controller = SteamVR_Controller.Input ((int)inputDevice.index);
			return controller.GetPressDown (Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);
		}
		return false;
	}
}
