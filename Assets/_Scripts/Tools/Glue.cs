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

        SteamVR_TrackedObject inputDevice = this.gameObject.GetComponentInParent<SteamVR_TrackedObject>();
        if (inputDevice != null && isPickedup)
        {
            SteamVR_Controller.Device controller = SteamVR_Controller.Input((int)inputDevice.index);
            if (controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger) && Time.time - wait > 1)
            {
                GameObject thisglue = (GameObject)Instantiate(glue, this.transform.position + this.transform.forward * (float)0.2, Quaternion.identity);
                wait = Time.time;
            }
        }

        if (Input.GetKey("g") && Time.time - wait > 1 && isPickedup)
        {
            GameObject thisglue = (GameObject)Instantiate(glue, this.transform.position + this.transform.forward * (float)0.2, Quaternion.identity);
            wait = Time.time;
        }
    }
}
