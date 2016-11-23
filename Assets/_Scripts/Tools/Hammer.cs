using UnityEngine;
using System.Collections;

public class Hammer : Tool {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void doAction(GameObject g)
    {
        Debug.Log("Hammer object");
        var transform = g.transform;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z / 2);
    }
}
