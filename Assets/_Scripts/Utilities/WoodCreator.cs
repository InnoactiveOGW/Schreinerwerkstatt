using UnityEngine;
using System.Collections;

public class WoodCreator : Pickup {

    public Transform wood;
    public Vector3 position;

    public void createNewWood() {
        Debug.Log("trying to create new wood");
        var gameObjects = GameObject.FindGameObjectsWithTag("Wood");
        for (var i = 0; i < gameObjects.Length; i++)
            Destroy(gameObjects[i]);
        Instantiate(wood, position, Quaternion.identity);
    }
}
