using UnityEngine;
using System.Collections;

public class NailHeadController : MonoBehaviour {

	void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Wood")
        {
            TestNailController tnc = GetComponentInParent<TestNailController>();
            if (tnc)
            {
                tnc.explode();
            }
        }
    }
}
