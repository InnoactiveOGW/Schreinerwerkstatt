using UnityEngine;
using System.Collections;

public class ControllerCube : MonoBehaviour {

    Color activeColor = new Color(0.5f, 0.4f, 0.4f);
    Color inactiveColor = new Color(0.2f, 0.2f, 0.2f);
    Color colorDelta;
    Color targetColor;
    bool changeColor = false;



    Renderer currentRenderer;
    Material currentMaterial;

    public float pickupRadius = 1f;
    public Pickup pickedObject;

    public Animation handAnimation;
    public Interactable selectedObject;
	HapticFeedbackController hfc;
	SteamVR_TrackedObject inputDevice;
	SteamVR_Controller.Device controller
	{
		get
		{   if (inputDevice != null)
                return SteamVR_Controller.Input((int)inputDevice.index);
            else
                return null;
		}
	}

    // Use this for initialization
    void Start () {
		inputDevice = GetComponentInParent<SteamVR_TrackedObject>();
		hfc = GetComponentInParent<HapticFeedbackController>();

        currentRenderer = gameObject.GetComponentInChildren<Renderer>();
        currentMaterial = currentRenderer.material;
        currentMaterial.color = inactiveColor;
    }
	
	// Update is called once per frame
	void Update () {
        var triggerButton = false;
        var gripButton = false;
        if (Config.isVR)
        {
            triggerButton = controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);
            gripButton = controller.GetPressUp(Valve.VR.EVRButtonId.k_EButton_Grip);
        }

        if (pickedObject == null && (triggerButton || Input.GetMouseButtonDown(0)))
            {
                if (selectedObject != null && selectedObject is Pickup)
                {
                    (selectedObject as Pickup).GetPickedUp(gameObject, out pickedObject);
                    Rigidbody rb = pickedObject.GetComponent<Rigidbody>();
                    if (rb)
                    {
                        rb.constraints = RigidbodyConstraints.FreezeAll;
                    }
                }
                else if (selectedObject != null)
                {
                    selectedObject.interact(this.gameObject);
                }
                else {
                    handAnimation.CrossFade("GrabEmpty");
                    handAnimation.CrossFadeQueued("ReverseGrabEmpty");
                }
                return;
            }
            else if (pickedObject != null && gripButton)
            {
                Rigidbody rb = pickedObject.GetComponent<Rigidbody>();
                if (rb)
                {
                    rb.constraints = RigidbodyConstraints.None;
                }
                pickedObject.GetReleased(new Vector3(0, 0, 0));
                pickedObject = null;
                handAnimation.CrossFade("ReverseGrabEmpty");
                return;
            }
        else {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                Vector3 direction = gameObject.transform.position - gameObject.transform.parent.position;
                gameObject.transform.position += direction * 0.1f;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                Vector3 direction = gameObject.transform.position - gameObject.transform.parent.position;
                gameObject.transform.position -= direction * 0.1f;
            }
        }

        if (changeColor && colorDelta != null)
        {
            currentMaterial.color += colorDelta * 0.1f;
            if (currentMaterial.color == targetColor)
            {
                changeColor = false;
            }
        }
    }

    void OnTriggerEnter(Collider collider) {
        if (pickedObject != null)
            return;
        Interactable p = collider.gameObject.GetComponent<Interactable>();
        if(p != null) {
            colorDelta = activeColor - currentMaterial.color;
            changeColor = true;
            targetColor = activeColor;
            handAnimation.CrossFade("Point");
            selectedObject = p;
        }
    }

    void OnTriggerExit(Collider collider) {
        if (pickedObject != null)
            return;
        if (selectedObject != null && selectedObject.gameObject == collider.gameObject)
        {
            colorDelta = inactiveColor - currentMaterial.color;
            changeColor = true;
            targetColor = inactiveColor;
            handAnimation.CrossFade("ReversePoint");
            selectedObject = null;
        }
    }
}
