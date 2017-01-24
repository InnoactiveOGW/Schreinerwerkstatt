using UnityEngine;
using System.Collections;

public class SpawnWood : MonoBehaviour {

    public GameObject woodCubePrefab;
    public CuttingGameController gameCtrl;
	
	// Update is called once per frame
	void Update () {

        if (!gameCtrl.playing)
            return;

        GameObject[] woods = GameObject.FindGameObjectsWithTag("WoodCube");

        if (gameCtrl.playing && woods.Length < gameCtrl.maxPieces) {
            Debug.Log("spawning new wood");
            float newRandom = Random.value;
            if (newRandom < gameCtrl.spawnRate)
            {
                GameObject woodCube = (GameObject)Instantiate(woodCubePrefab, gameObject.transform.position + new Vector3(0,0.5f,0), gameObject.transform.rotation);

                Rigidbody rb = woodCube.GetComponent<Rigidbody>();
                float newRandom2 = Random.value;
                if (rb != null) { 
                    rb.AddForce(new Vector3(0, newRandom2 * gameCtrl.speed, 0));
                    // rb.mass = gameCtrl.speed;
                }
            }
        }
	}
}
