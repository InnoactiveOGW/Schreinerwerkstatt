using UnityEngine;
using System.Collections;
using System;

public class TutorialLevel : LevelCtrl{

    public bool spawnLvl = true;
    public GameObject blueprintParent;
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
            startLevel();
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
        GameObject bp = Instantiate(blueprint);
        bp.transform.SetParent(blueprintParent.transform);
    }

    private void spawnMaterials()
    {
        foreach (GameObject g in materials)
        {
            Instantiate(g);
        }
    }

    public override void startLevel()
    {
        base.startLevel();
        spawnLevel1();
    }

    public override void resetLevel()
    {
        base.resetLevel();
        cleanupLevel1();
        spawnLevel1();
    }

    public override void endLevel()
    {
        base.endLevel();
        cleanupLevel1();
    }

    private void cleanupLevel1()
    {
        // TODO
    }
}
