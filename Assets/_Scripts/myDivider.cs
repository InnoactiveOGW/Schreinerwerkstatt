using UnityEngine;
using System.Collections;

public class myDivider : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }


    void OnTriggerEnter(Collider other)
    {
        Tool t = other.GetComponent<Tool>();
        if (t) {
            Debug.Log("tool used: " + t.ToString());
            t.doAction(this.gameObject);
        }
    }
}
