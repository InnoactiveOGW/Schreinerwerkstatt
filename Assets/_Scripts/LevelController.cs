using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class LevelController : LevelControllerInterface{

    public bool spawnLvl = true;
    public GameObject blueprintParent;
    public GameObject blueprint;
    public GameObject[] materials;

    public GameObject[] tools;

    Vector3 originalPosition;
    Quaternion orginalRotation;


    public GameObject Step1;
    public GameObject Step2;
    public GameObject Step3;
    public GameObject Step4;
    public GameObject Step5;
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
        Debug.Log("Reseting current Level");
        base.resetLevel();
        cleanupLevel1();
        spawnLevel1();
    }

    public override void endLevel()
    {
        Debug.Log("Exiting current Level");
        base.endLevel();
        cleanupLevel1();
    }

    private void cleanupLevel1()
    {
        // TODO
        GameObject[] wood = GameObject.FindGameObjectsWithTag("Wood");
        foreach (GameObject w in wood)
            Destroy(w);

        GameObject[] glue = GameObject.FindGameObjectsWithTag("Glue");
        foreach (GameObject g in glue)
            Destroy(g);

        GameObject[] tools = GameObject.FindGameObjectsWithTag("Tool");
        foreach (GameObject t in tools)
            Destroy(t.gameObject);
        foreach(GameObject tool in this.tools)
        {
            Instantiate(tool);
        }
        GameObject[] blueprint = GameObject.FindGameObjectsWithTag("Blueprint");
        foreach (GameObject bp in blueprint)
            Destroy(bp);
    }
}
