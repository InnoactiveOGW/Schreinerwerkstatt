using UnityEngine;
using System.Collections;
using System;

public class TutorialLevel : MonoBehaviour {

    public bool spawnLvl = true;
    public GameObject blueprint;
    public GameObject[] materials;
    Vector3 originalPosition;
    Quaternion orginalRotation;

	// Use this for initialization
	void Start () {
        originalPosition = gameObject.transform.position;
        orginalRotation = gameObject.transform.rotation;
    }
	

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Workspace" && spawnLvl)
        {
            spawnLevel1();
            Pickup pu = GetComponent<Pickup>();
            if(pu != null)
            {
                pu.GetReleased();
            }

            this.gameObject.transform.position = originalPosition;
            this.gameObject.transform.rotation = orginalRotation;
        }
    }

    private void spawnLevel1()
    {
        spawnLvl = false;
        spawnBlueprint();
        spawnMaterials();
    }

    void spawnBlueprint()
    {
        Instantiate(blueprint);
    }

    private void spawnMaterials()
    {
        foreach (GameObject g in materials)
        {
            Instantiate(g);
        }
    }

    public void restartLevel()
    {
        cleanupLevel1();
        spawnLevel1();
    }

    private void cleanupLevel1()
    {
        // TODO
    }
}
