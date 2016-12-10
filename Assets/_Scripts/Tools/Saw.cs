using UnityEngine;
using System.Collections;

public class Saw : Tool {

    bool isSawing = false;
    Divider wood = null;
    GameObject oldParent = null;
    Rigidbody rigidbody;
    // Use this for initialization
    void Start () {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        if (rigidbody == null)
        {
            throw new MissingComponentException("No rigidbody was found on the saw but there needs to be a rigidbody on the saw!");
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("r") && isSawing)
        {

            transform.position += transform.forward * 0.01f;

        }
        else if (Input.GetKey("f") && isSawing)
        {

            transform.position -= transform.forward * 0.01f;
            transform.position -= transform.up * 0.005f;

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
            if (contact.otherCollider == collision.collider && wood != null && wood.canBeDivided)
            {
                enterSawingMode();
            }
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (isSawing) {
            isSawing = false;
            wood = null;
            oldParent = null;
            GetPickedUp(oldParent);
            rigidbody.isKinematic = false;
        }
    }

    void enterSawingMode()
    {
        isSawing = true;
        rigidbody.isKinematic = true;
        oldParent = gameObject.transform.parent.gameObject;
        GetReleased();
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.01f, gameObject.transform.position.z);
    }


    
}
