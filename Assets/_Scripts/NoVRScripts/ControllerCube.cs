﻿using UnityEngine;
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
    public Pickup selectedObject;

    // Use this for initialization
    void Start () {
        currentRenderer = gameObject.GetComponentInChildren<Renderer>();
        currentMaterial = currentRenderer.material;
        currentMaterial.color = inactiveColor;

        //handAnimation = gameObject.GetComponent<Animation>();
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Vector3 direction = gameObject.transform.position - gameObject.transform.parent.position;
            //direction.y = 0;
            gameObject.transform.position += direction * 0.1f;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0) {
            Vector3 direction = gameObject.transform.position - gameObject.transform.parent.position;
            //direction.y = 0;
            gameObject.transform.position -= direction * 0.1f;
        }

        if(changeColor && colorDelta != null)
        {
            currentMaterial.color += colorDelta * 0.1f;
            if (currentMaterial.color == targetColor) {
                changeColor = false;
            }
        }

        if (pickedObject == null && Input.GetMouseButtonDown(0))
        {
            if(selectedObject != null)
            {
                selectedObject.GetPickedUp(gameObject, out pickedObject);
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

                // pickedObject = selectedObject;
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
    }

    void OnTriggerEnter(Collider collider) {
        Pickup p = collider.gameObject.GetComponent<Pickup>();
        if(p != null) { 
            colorDelta = activeColor - currentMaterial.color;
            changeColor = true;
            targetColor = activeColor;
            handAnimation.CrossFade("Point");
            selectedObject = p;
        }
    }

    void OnTriggerExit() {
        colorDelta =  inactiveColor - currentMaterial.color;
        changeColor = true;
        targetColor = inactiveColor;
        handAnimation.CrossFade("ReversePoint");
        selectedObject = null;
    }
}
