using UnityEngine;
using System.Collections;

public class Tool : Pickup
{
    public virtual void doAction(GameObject g) {
        Debug.Log("Tool does some action ...");
    }
}
