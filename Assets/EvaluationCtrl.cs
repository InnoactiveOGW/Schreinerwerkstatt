using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EvaluationCtrl : MonoBehaviour {

    public bool isRateable;

    public float glueWoodDistanceFactor = 1000;
    public float woodSizeFactor = 5;
    public float woodDegreeFactor = 10;

    public float difficultyGlue = 1;
    public float difficultyGlueToWood = 1;
    public float difficultyWood = 1;

    public float rate()
    {
        return evaluateConstruction(this.gameObject);
    }

    public float evaluateConstruction(GameObject go)
    {
        float result = 0;
        Transform[] children = go.GetComponentsInChildren<Transform>();

        List<Transform> glueTargets = new List<Transform>();
        List<Transform> glues = new List<Transform>();
        List<Transform> wood = new List<Transform>();

        foreach (Transform t in children)
        {

            if (t.gameObject.tag == "GlueTarget")
            {
                glueTargets.Add(t);
            }
            else if (t.gameObject.tag == "Wood")
            {
                if (t.gameObject.name.Contains("chairtop"))
                {
                }
                else
                {
                    wood.Add(t);
                }
            }
            else if (t.gameObject.tag == "Glue")
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
        // Evaluate the size of the wood pieces
        // more similarity => better score
        if (!(wood.Count > 0)) return 0;
        Mesh lastMesh = wood[wood.Count - 1].gameObject.GetComponent<MeshFilter>().sharedMesh;
        foreach (Transform w in wood)
        {

            GameObject gameObj = w.gameObject;
            MeshFilter mf = w.gameObject.GetComponent<MeshFilter>();
            if (lastMesh != null && mf != null)
            {
                // TODO: check
                result += (mf.sharedMesh.bounds.size - lastMesh.bounds.size).magnitude / woodSizeFactor;
            }
            lastMesh = mf.sharedMesh;
        }

        // Evaluate the angle of the wood pieces
        // best: 90 degree, worst 0 / 180
        var tempResult = 0.0f;
        var temptempResult = 0.0f;
        foreach (Transform w in wood)
        {
            temptempResult = 1 - Mathf.Abs(Vector3.Dot(w.forward.normalized, go.transform.up.normalized));
            tempResult += temptempResult * woodDegreeFactor;
        }
        result += tempResult;

        return (result > 0 ? 0 : result * -1) * difficultyWood;
    }

    private float evaluateGlueToWood(List<Transform> glues, List<Transform> wood)
    {
        float result = -1;
        Vector3 distanceVec = new Vector3(0, 0, 0);
        foreach (Transform g in glues)
        {
            float minDis = -1;
            foreach (Transform w in wood)
            {
                distanceVec = new Vector3(0, 0, 0);
                Vector3 vecToGlue = g.position - w.position;
                RaycastHit hit;

                Vector3 forwardNorm = w.forward.normalized;
                Vector3 vecToGlueNorm = vecToGlue.normalized;

                // TODO
                // checken!!
                if (Physics.Raycast(
                    w.position,
                    Vector3.Dot(forwardNorm, vecToGlueNorm) > 0 ? w.forward : -w.forward,
                    out hit,
                    float.PositiveInfinity,
                    LayerMask.GetMask("GlueLayer"),
                    QueryTriggerInteraction.Ignore))
                {
                    //if (Physics.Raycast(w.position, Vector3.Dot (w.up, vecToGlue) < 0 ? w.up : -w.up, out hit)) {
                    Vector3 vecToHitPoint = g.position - hit.point;
                    float distanceToHitPoint = vecToHitPoint.sqrMagnitude;
                    if (distanceToHitPoint < minDis || minDis == -1)
                    {
                        minDis = distanceToHitPoint;
                        distanceVec = vecToHitPoint;
                    }
                }
                if (distanceVec != null && distanceVec.magnitude > 0)
                    result += distanceVec.magnitude * glueWoodDistanceFactor;
            }
        }

        // Result wird umgedreht da es eine Fehlerfunktion dargestellt hat, danach ist es ein %-korrekt Wert
        return (result > 0 ? 0 : result * -1) * difficultyGlueToWood;
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
        /*
			bestcase: 0 Distanz => result == -1 <=> 100% korrekt
			falls result > 0 => kummulierte Distanz war größer als 1 => 0%
		*/
        return (result > 0 ? 0 : result * -1) * difficultyGlue;
    }
}
