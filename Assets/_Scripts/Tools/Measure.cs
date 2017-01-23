using UnityEngine;
using System.Collections;
using System;

public class Measure : Tool
{
    GameObject myLine;

    Vector3 startPoint;
    TextMesh valueText;

    AudioSource audioMeasure;
    float lastDistance;
    // Use this for initialization
    void Start()
    {
        valueText = this.GetComponentInChildren<TextMesh>();
        isPickedup = false;
        audioMeasure = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
		bool triggerButton = getTriggerButton();

		if ((Input.GetKey("g") || triggerButton) && isPickedup)
        {

            if (myLine == null)
            {
                myLine = new GameObject();
                myLine.AddComponent<LineRenderer>();
                LineRenderer lr = myLine.GetComponent<LineRenderer>();
                lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
                lr.SetColors(Color.yellow, Color.yellow);
                lr.SetWidth(0.01f, 0.01f);
                startPoint = this.gameObject.transform.position;
                lr.SetPosition(0, startPoint);
            }
            Vector3 endpoint = this.gameObject.transform.position;
            float currentDistance = calculateDistance(startPoint, endpoint);
            valueText.text = currentDistance.ToString();
            if (!audioMeasure.isPlaying && currentDistance != lastDistance)
                audioMeasure.Play();
            DrawLine(endpoint);
        }
        else
        {
            Destroy(myLine);
            valueText.text = "";
        }


    }

    private float calculateDistance(Vector3 startPoint, Vector3 endpoint)
    {
        return (((startPoint - endpoint).magnitude) * Config.sizeFactor);
    }

    void DrawLine(Vector3 end)
    {
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.SetPosition(1, end);
    }

	bool getTriggerButton() {
		SteamVR_TrackedObject inputDevice = this.gameObject.GetComponentInParent<SteamVR_TrackedObject> ();
		if (inputDevice != null && isPickedup) {
			SteamVR_Controller.Device controller = SteamVR_Controller.Input ((int)inputDevice.index);
			return controller.GetPress (Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);
		}
		return false;
	}
}
