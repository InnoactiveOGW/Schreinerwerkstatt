using UnityEngine;
using System.Collections;

public class Lightsaber : MonoBehaviour {


    public GameObject model;
    public GameObject blade;

    public GameObject modelPrefab;
    public GameObject bladePrefab;

    bool activated = false;
	
	// Update is called once per frame
	void Update ()
    {
        var triggerButton = false;
        Pickup pu = GetComponent<Pickup>();
        if (pu && pu.isPickedup)
        {
            SteamVR_Controller.Device controller = getController();
            if (controller != null)
                triggerButton = controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);
        }

        if (triggerButton || Input.GetMouseButtonDown(0))
        {
            activated = !activated;
            if (activated)
            {
                model.SetActive(true);
                blade.SetActive(true);
            }
            else {
                model.SetActive(false);
                blade.SetActive(false);
            }
        }
    }

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
}
