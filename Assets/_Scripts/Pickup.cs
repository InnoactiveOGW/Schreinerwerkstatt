using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour
{
    public void GetPickedUp(GameObject byThisObject)
    {
        Transform tempParent = this.gameObject.transform;
        tempParent.SetParent(byThisObject.transform);
    }

    public void GetPickedUp(GameObject byThisObject, out Pickup parent)
    {

        // Transform nextParent = this.gameObject.transform.parent;
        Transform tempParent = this.gameObject.transform;
        if (gameObject.tag == "Wood")
            while (tempParent.parent != null)
            {
                tempParent = tempParent.parent;
            }

        Pickup pu = tempParent.GetComponent<Pickup>();
        parent = pu;
        tempParent.SetParent(byThisObject.transform);
    }

    public void GetReleased()
    {
        ControllerCube cc = GetComponentInParent<ControllerCube>();
        if (cc != null && cc.pickedObject == this)
            cc.pickedObject = null;
        transform.SetParent(null);
    }

    public void GetReleased(Vector3 velocity)
    {
        this.GetReleased();
        GetComponent<Rigidbody>().velocity = velocity;
    }

    public virtual void doAction(GameObject g)
    {
        Debug.Log("Normal pickup object");
    }
}
