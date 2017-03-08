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
                
            if (pickedObject == null && triggerButton )
            {
                if (selectedObject != null && selectedObject is Pickup)
                {
                    (selectedObject as Pickup).GetPickedUp(gameObject, out pickedObject);
                    Rigidbody rb = pickedObject.GetComponent<Rigidbody>();
                    if (rb)
                    {
                        rb.constraints = RigidbodyConstraints.FreezeAll;
                        // rb.useGravity = false;
                    }

                    //if (selectedObject.tag == "Tool")
                    //{
                    //    pickedObject.transform.position = this.gameObject.transform.position;
                    //    pickedObject.transform.rotation = this.gameObject.transform.rotation;
                    //}
                    //handAnimation.CrossFade("GrabEmpty");
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

        // deprecated / non-VR Code, needs cleanup

		if (pickedObject == null && Input.GetMouseButtonDown(0))
        {
            if(selectedObject != null)
            {
                if (selectedObject is Pickup) {
                    (selectedObject as Pickup).GetPickedUp(gameObject, out pickedObject);
                    Rigidbody rb = pickedObject.GetComponent<Rigidbody>();
                    if (rb)
                    {
                        rb.constraints = RigidbodyConstraints.FreezeAll;
                        // rb.useGravity = false;
                    }
                    if(selectedObject.tag == "Tool")
                    {
                        pickedObject.transform.position = this.gameObject.transform.position;
                        pickedObject.transform.rotation = this.gameObject.transform.rotation;
                    }
                } else
                {
                    selectedObject.interact(this.gameObject);
                }
                return;
            }

            handAnimation.CrossFade("GrabEmpty");
            
            Collider[] pickups = Physics.OverlapSphere(transform.position, pickupRadius);
            if (pickups != null)
            {
                Collider nearestCollider = null;
                float minDis = pickupRadius;
                foreach (Collider collider in pickups)
                {
                    var currDis = Vector3.Distance(collider.transform.position, transform.position);
                    Pickup pickup = collider.GetComponent<Pickup>();
                    if (currDis < minDis && pickup != null)
                    {
                        minDis = currDis;
                        nearestCollider = collider;
                    }
                }

                if (nearestCollider)
                {
                    WoodCreator wc = nearestCollider.GetComponent<WoodCreator>();
                    if (wc != null)
                    {
                        wc.createNewWood();
                        Renderer wcRend = wc.gameObject.GetComponent<Renderer>();
                        Material wcMat = wcRend.material;
                        wcMat.color = new Color(Random.value, Random.value, Random.value);
                        return;
                    }

                    Tool tool = nearestCollider.GetComponent<Tool>();
                    Pickup pickup = null;
                    if (tool == null)
                    {
                        pickup = nearestCollider.GetComponent<Pickup>();
                        if (pickup)
                        {
                            pickup.GetPickedUp(gameObject);
                            Rigidbody rb = pickup.GetComponent<Rigidbody>();
                            if (rb)
                            {
                                rb.constraints = RigidbodyConstraints.FreezeAll;
                                // rb.useGravity = false;
                            }
                            pickedObject = pickup;
                        }
                    }
                    else
                    {
                        tool.GetPickedUp(gameObject);
                        tool.transform.rotation = transform.rotation;
                        tool.transform.localPosition = new Vector3(0, 0, 0);
                        Rigidbody rb = tool.GetComponent<Rigidbody>();
                        if (rb)
                        {
                            rb.constraints = RigidbodyConstraints.FreezeAll;
                            //rb.useGravity = false;
                        }
                        pickedObject = tool;
                    }
                }
            }
            handAnimation.CrossFadeQueued("ReverseGrabEmpty");
        }
		else if (pickedObject != null && Input.GetMouseButtonDown(1)) {
            
            Rigidbody rb = pickedObject.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.constraints = RigidbodyConstraints.None;
            }
            pickedObject.GetReleased(new Vector3(0, 0, 0));
            pickedObject = null;
            handAnimation.CrossFadeQueued("ReverseGrabEmpty");
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
            // Debug.Log("Found interactable");
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
