using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour
{
    public void GetPickedUp(GameObject byThisObject)
    {
        transform.SetParent(byThisObject.transform);
    }

    public void GetReleased()
    {
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
