using UnityEngine;
using System.Collections;

public class ColliderTriggerBehavior : MonoBehaviour {

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Wood")
        {
            Saw s = transform.parent.GetComponent<Saw>();
            if(s != null)
            {
                Debug.Log("trigger enter");
                s.triggerEntered = true;
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Wood")
        {
            Saw s = transform.parent.GetComponent<Saw>();
            if (s != null)
            {
                Debug.Log("trigger exit");
                s.triggerEntered = false;
            }
        }
    }
}
