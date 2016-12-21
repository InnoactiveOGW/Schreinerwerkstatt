using UnityEngine;
using System.Collections;

public class Saw : Tool {

    bool isSawing = false;
    Divider wood = null;
    GameObject oldParent = null;
    new Rigidbody rigidbody;
    public float power = 0.0005f;
    public float movementDelay = 0.005f;
    // Use this for initialization
    void Start () {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        if (rigidbody == null)
        {
            throw new MissingComponentException("No rigidbody was found on the saw but there needs to be a rigidbody on the saw!");
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetKey("r") && isSawing)
        {

            transform.position += transform.forward * 0.01f;

        }
        else if (Input.GetKey("f") && isSawing)
        {

            transform.position -= transform.forward * 0.01f;
            transform.position -= transform.up * 0.005f;

        }

        if (isSawing)
        {
            SteamVR_TrackedObject inputDevice = oldParent.GetComponent<SteamVR_TrackedObject>();
            SteamVR_Controller.Device controller = SteamVR_Controller.Input((int)inputDevice.index);

            // if (controller != null && Mathf.Abs(controller.velocity.z) > 0.3)
            if (controller != null)
            {
                Vector3 normalizedVelocity = controller.velocity;
                transform.position += transform.forward * Vector3.Dot(transform.forward, controller.velocity) * movementDelay;
                transform.position +=  transform.up * Mathf.Abs(Vector3.Dot(transform.forward, controller.velocity)) * -power;
                //Debug.Log("controller.velocity: " + controller.velocity.ToString());
                //Debug.Log("normalizedVelocityy: " + normalizedVelocity.ToString());
            }

        }
    }

    public override void doAction(GameObject g)
    {
        Debug.Log("Saw object collided");
        // var transform = g.transform;
        // transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z / 2);

        // 1: Enter Sawing mode
        // enterSawingMode();

        // TODO 2.1: user performs sawing motion => 3.

        // TODO 2.2: user can exit sawing mode => TODO: how is this performed?

        // TODO 3: saw gets moved throw the wood

        // 4: on collision exit => sawing is completed, wood gets cut (is done by the WoodDivider Script, doesnt need to be touched), sawing mode exit 

        // TODO: modelling the user interaction with the saw while the saw is cutting the wood


    }

    public void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            wood = collision.gameObject.GetComponent<Divider>();
            if (contact.otherCollider == collision.collider && wood != null && wood.canBeDivided && oldParent == null)
            {
                enterSawingMode();
            }
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (isSawing && (wood.initialContactPoint.y - collision.transform.position.y) > 0.5)
        {
            //GetPickedUp(oldParent);
            isSawing = false;
            wood = null;
            //oldParent = null;
            rigidbody.isKinematic = false;
        }
    }

    void enterSawingMode()
    {
        Debug.Log("sawing mode entered");
        isSawing = true;
        rigidbody.isKinematic = true;
        oldParent = gameObject.transform.parent.gameObject;
        GetReleased();
        //gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.01f, gameObject.transform.position.z);
    }


    
}
