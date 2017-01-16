using UnityEngine;
using System.Collections;

public class WoodCreator : Pickup {

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
        var gameObjects = GameObject.FindGameObjectsWithTag("Wood");

        for (var i = 0; i < gameObjects.Length; i++)
            Destroy(gameObjects[i]);

        Instantiate(wood, position, Quaternion.identity);
    }
}
