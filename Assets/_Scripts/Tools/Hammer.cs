using UnityEngine;
using System.Collections;

public class Hammer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //public override void doAction(GameObject g)
    //{
    //    //Debug.Log("Hammer object");
    //    //var transform = g.transform;
    //    //transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z / 2);
    //}


    void OnTriggerEnter(Collider collider) {
        NailController nc = collider.gameObject.GetComponent < NailController >();
        if (nc != null && nc.canBeNailed)
        {
            Rigidbody rb = nc.gameObject.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;

                // Problem: rigidbody wird nicht direkt benutzt um Position des Objekts zu verändern => entweder Geschwindigkeit 
                // selbst berechnen oder Rigidbody verwenden (kann zu sehr merkwürdigen Ergebnissen führen!)
                float force = 1;
                Rigidbody thisRB = gameObject.GetComponent<Rigidbody>();
                if (thisRB)
                    force = thisRB.velocity.magnitude;
                force = force > 0 ? force : 1;
                Debug.Log("force: " + force);
                nc.getPinnedToWood(force);
            }

            // TODO Verformung des Nagels bei schlechtem Treffen -> Vektoren zum bewerten: hammer.up, nail.up -> sollten im rechten Winkel zueinander stehen
        }
    }
}
