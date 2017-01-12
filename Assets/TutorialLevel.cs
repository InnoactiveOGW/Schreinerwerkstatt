using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TutorialLevel : LevelCtrl{

    public bool spawnLvl = true;
    public GameObject blueprintParent;
    public GameObject blueprint;
    public GameObject[] materials;

    public GameObject[] tools;

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

    public override float evaluateConstruction(GameObject go)
    {
        float result = 0;
        Transform[] children = go.GetComponentsInChildren<Transform>();

        List<Transform> glueTargets = new List<Transform>();
        List<Transform> glues = new List<Transform>();
        List<Transform> wood = new List<Transform>();

        foreach (Transform t in children)
        {

            if(t.gameObject.tag == "GlueTarget"){
                glueTargets.Add(t);
            }
            else if(t.gameObject.tag == "Wood")
            {
                wood.Add(t);
            }
            else if(t.gameObject.tag == "Glue")
            {
                glues.Add(t);
            }
        }

        float glueEvaluation = evaluateGlue(glueTargets, glues);
        float woodToGlueEvaluation = evaluateGlueToWood(glues, wood);
        float woodEvaluation = evaluateWood(wood, go);
        Debug.Log("glueEvaluation: " + glueEvaluation);
        Debug.Log("woodToGlueEvaluation: " + woodToGlueEvaluation);
        Debug.Log("woodEvaluation: " + woodEvaluation);

        result = 1000 - glueEvaluation - woodToGlueEvaluation - woodEvaluation;
        Debug.Log("result: " + result);

        return result;
    }

    private float evaluateWood(List<Transform> wood, GameObject go)
    {
        float result = -1;
        Mesh lastMesh = wood[wood.Count - 1].gameObject.GetComponent<MeshFilter>().sharedMesh;
        foreach (Transform w in wood)
        {
            // Evaluate the size of the wood pieces
            GameObject gameObj = w.gameObject;
            MeshFilter mf = w.gameObject.GetComponent<MeshFilter>();
            if(lastMesh != null && mf != null)
            {
                // TODO: check
               result += (mf.sharedMesh.bounds.size - lastMesh.bounds.size).magnitude;
            }

            lastMesh = mf.sharedMesh;

            // Evaluate the angle of the wood pieces
            // TODO: configuration!
            // TODO: check
            result += Vector3.Dot(w.forward, go.transform.up);
        }

        return result;
    }

    private float evaluateGlueToWood(List<Transform> glues, List<Transform> wood)
    {
        float result = -1;

        Vector3 distanceVec = new Vector3(0,0,0);
        foreach (Transform g in glues)
        {
            float minDis = -1;
            foreach(Transform w in wood)
            {
                Vector3 tempVec = g.position - w.position;
                float distance = tempVec.magnitude;
                if(distance < minDis || minDis == -1)
                {
                    // TODO: check
                    minDis = distance;
                    distanceVec = tempVec;
                }
            }
            // TODO: check
            if (distanceVec != null && distanceVec.magnitude > 0)
                result = distanceVec.x + distanceVec.z;
        }

        return result;
    }

    float evaluateGlue(List<Transform> glueTargets, List<Transform> glues)
    {
        float result = -1;
        foreach (Transform gt in glueTargets)
        {
            float minDis = -1;
            foreach (Transform g in glues)
            {
                float distance = (gt.position - g.position).magnitude;
                if (distance < minDis || minDis == -1)
                {
                    minDis = distance;
                }
            }
            // TODO: check
            result += minDis;
        }
        return result;
    }
}
