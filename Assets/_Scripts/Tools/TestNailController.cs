using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestNailController : NailController {

    //public Transform parent;
    //public bool canBeNailed = false;
    //public bool preventProgress = false;
    //public List<GameObject> woods = null;

    bool firstHit = true;
    static System.Random rnd = new System.Random();
    Transform modBone;

    public Color bestScoreColor;
    public Color noScoreColor;
    public Color startColor;
    public Color currentColor;
    public Material currentMaterial;
    public Color targetColor;
    public Color colorDelta;
    public bool changeColor = false;

    public float score = 0;


    void Start() {
        woods = new List<GameObject>();

        Transform[] bones = GetComponentsInChildren<Transform>();
        List<Transform> tempBones = new List<Transform>();
        foreach (Transform bone in bones)
        {
            if (bone.tag == "Amature")
                continue;
            tempBones.Add(bone);
        }

        int i = rnd.Next(tempBones.Count);
        modBone = tempBones[i];

        currentMaterial = GetComponent<Renderer>().material;
        startColor = currentMaterial.color;
        currentColor = startColor;
    }

    void Update()
    {
        changeToColor();
    }

    public void explode()
    {
        // TODO
        Debug.Log("nail exploded");
        preventProgress = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
        {
            Destroy(rb);
        }

        setNewColor(noScoreColor);

        CuttingGameController cgc = FindObjectOfType<CuttingGameController>();
        if (cgc != null)
        {
            cgc.addMiss(1);
        }
    }

    void OnTriggerEnter(Collider collision) {
        if(collision.gameObject.tag == "Wood" && !preventProgress)
        {
            woods.Add(collision.gameObject);
            canBeNailed = true;
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            setNewColor(noScoreColor);
        }
    }

    void setNewColor(Color newTargetColor)
    {
        targetColor = newTargetColor;
        colorDelta = targetColor - currentMaterial.color;
        changeColor = true;
    }

    void changeToColor()
    {
        if (changeColor && colorDelta != null)
        {
            currentMaterial.color += colorDelta * 0.1f;
            if (currentMaterial.color == targetColor)
            {
                changeColor = false;
            }
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (woods.Contains(collision.gameObject))
        {
            Debug.Log("nail lost contact with wood");
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            if(rb != null) {
                rb.isKinematic = false;
            //    woodRB.mass = oldMass;
            //    woodRB.constraints = RigidbodyConstraints.None;
            //    woodRB.isKinematic = false;
            }
            woods.Remove(collision.gameObject);
            if (woods.Count == 0)
                canBeNailed = false;

            setNewColor(startColor);
        }
    }


    public override void getPinnedToWood(Vector3 velocity, Vector3 hammerPosition)
    {
        float force = velocity.magnitude;

        if (preventProgress) {
            Debug.Log("Nail cant progess anymore");
            return;
        }

        if (!canBeNailed)
            return;

        float dot = Vector3.Dot(-transform.up.normalized, velocity.normalized);
        Debug.Log("Piined Dot Product: " + dot);
        if (dot < 0.93)
        {
            float velocityFactor = 5;

            float cosX = Vector3.Dot(transform.right, velocity) * velocityFactor;
            float cosY = Vector3.Dot(transform.up, velocity) * velocityFactor;
            float cosZ = Vector3.Dot(transform.forward, velocity) * velocityFactor;

            modBone.Rotate(new Vector3(-cosZ, -cosY, -cosX));
            dot = (dot / 2) * velocity.sqrMagnitude;
            score = score - dot > 0 ? score - dot : 0;
        }
        else
        {
            float addScore = dot * force;

            CuttingGameController cgc = FindObjectOfType<CuttingGameController>();
            if (cgc != null)
            {
                cgc.score -= Mathf.RoundToInt(score);
            }

            score = score + addScore;
            if (cgc != null)
            {
                cgc.score += Mathf.RoundToInt(score);
            }
            transform.position = transform.position - transform.up * force * dot * 0.01f;

            
        }

        Color newDelta = (bestScoreColor - noScoreColor) * score / 15;
        setNewColor(newDelta + noScoreColor);
        Debug.Log("Score: " + score);
        
        if (firstHit) {
            Pickup pu = GetComponent<Pickup>();
            if (pu)
            {
                pu.GetReleased();
            }
            transform.SetParent(woods[0].transform);

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb)
            {
                rb.isKinematic = true;
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }

            MeshCollider[] mcs = GetComponents<MeshCollider>();
            foreach (MeshCollider mc in mcs)
            {
                if (!mc.isTrigger)
                {
                    Destroy(mc);
                }
            }
            firstHit = false;
        }

        //Debug.Log("pinning nail to wood");
        //foreach (GameObject wood in woods) {
        //    wood.transform.SetParent(null);
        //}
        //// old: transform.position = transform.position - transform.up * force * 0.001f;
        ////Pickup parentPickup = parent.GetComponent<Pickup>();
        ////parentPickup.GetReleased();

        // TODO

        //foreach (GameObject wood in woods)
        //{
        //    if(wood.transform.parent != parent)
        //        wood.transform.SetParent(parent);
        //}
    }
}
