using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour
{
    public void GetPickedUp(GameObject byThisObject)
    {
        transform.SetParent(byThisObject.transform);
    }

    public void GetReleased(Vector3 velocity)
    {
        transform.SetParent(null);
        GetComponent<Rigidbody>().velocity = velocity;
    }
}
