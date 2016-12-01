using UnityEngine;
using System.Collections;

public class WoodCreator : MonoBehaviour {

    public Transform wood;
    public Vector3 position;


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void createNewWood() {
        // TODO
        Debug.Log("trying to create new wood");

        Instantiate(wood, position, Quaternion.identity);
    }
}
