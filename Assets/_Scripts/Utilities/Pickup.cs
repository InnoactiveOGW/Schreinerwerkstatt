using UnityEngine;
using System.Collections;

public class Pickup : Interactable
{
    public bool isPickedup;
    Vector3 initPosition;
    Quaternion initRotation;

    void Start()
    {
        initPosition = transform.position;
        initRotation = transform.rotation;
    }

    public void Reset()
    {
        transform.position = initPosition;
        transform.rotation = initRotation;
    }

    public void GetPickedUp(GameObject byThisObject)
    {
        Transform tempParent = this.gameObject.transform;
        tempParent.SetParent(byThisObject.transform);
        isPickedup = true;
    }

    public void GetPickedUp(GameObject byThisObject, bool align)
    {
        if (align)
        {
            this.transform.position = byThisObject.transform.position + byThisObject.transform.forward.normalized * 0.55f - byThisObject.transform.right.normalized * 0.05f;
            this.transform.rotation = byThisObject.transform.rotation;
        }

        GetPickedUp(byThisObject);
    }

    public void GetPickedUp(GameObject byThisObject, out Pickup pickedObj)
    {
        Transform tempPickup = this.gameObject.transform;
        Pickup pu = this;
        while (tempPickup.parent != null)
        {
            Pickup tempPu = tempPickup.parent.GetComponent<Pickup>();
            if(tempPu != null)
            {
                pu = tempPu;
            }
            tempPickup = tempPickup.parent;
        }
        pu.gameObject.transform.SetParent(byThisObject.transform);
        pu.isPickedup = true;

        // set out parameter
        pickedObj = pu;
    }

    public void GetReleased()
    {
        ControllerCube cc = GetComponentInParent<ControllerCube>();
        if (cc != null && cc.pickedObject == this)
            cc.pickedObject = null;
        transform.SetParent(null);
        isPickedup = false;
    }

    public void GetReleased(Vector3 velocity)
    {
        this.GetReleased();
        GetComponent<Rigidbody>().velocity = velocity;
        isPickedup = false;
    }

    public virtual void doAction(GameObject g)
    {
        Debug.Log("Normal pickup object");
    }
}
