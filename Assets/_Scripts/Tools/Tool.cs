using UnityEngine;
using System.Collections;

public class Tool : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void doAction(GameObject g) {
        Debug.Log("Tool does some action ...");
    }
}
