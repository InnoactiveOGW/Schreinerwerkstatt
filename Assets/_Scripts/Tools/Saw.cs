using UnityEngine;
using System.Collections;

public class Saw : Tool {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public override void doAction(GameObject g)
    {
        Debug.Log("Saw object");
        var transform = g.transform;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z / 2);
    }

    
}
