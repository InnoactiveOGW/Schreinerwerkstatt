using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserBlade : MonoBehaviour {

    Vector3 initialContactPoint;
    Vector3 initialContactPosition;
    Vector3 initialUpVector;
    Quaternion initialRotation;
    Vector3 initialRightVector;
    GameObject cuttee;
    List<GameObject> cuttees;

    public Material defaultCapMaterial;
    GameObject blade;
    bool isSawing = false;
    Divider wood = null;
    GameObject oldParent = null;
    public float power = 0.0005f;
    public float movementDelay = 0.005f;
    public Transform parentTransform;

    int hitCount = 0;
    CapsuleCollider thisCollider;

    //Audio
    AudioSource sawForward;
    AudioSource sawBack;

    // Use this for initialization
    void Start () {
        parentTransform = gameObject.transform.parent.transform;
        AudioSource[] audio = GetComponents<AudioSource>();
        if (audio.Length > 1)
        {
            sawForward = audio[0];
            sawBack = audio[1];
        }
        thisCollider = GetComponent<CapsuleCollider>();
        cuttee = null;
    }
	
	// Update is called once per frame
	void Update () {
        if (hitCount > 0 && cuttee == null) { 
            hitCount = 0;
            thisCollider.isTrigger = false;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.tag.Contains("Wood") 
            || collision.gameObject.tag == "WoodCubeCut" 
            || cuttee != null)
            return;

        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.otherCollider == collision.collider)
            {
                initialContactPoint = contact.point;
                initialUpVector = this.gameObject.transform.up;
                Vector3 obpos = this.gameObject.transform.position;
                initialContactPosition = obpos;
                initialRotation = this.gameObject.transform.rotation;
                initialRightVector = collision.gameObject.transform.rotation * this.gameObject.transform.right; // this.gameObject.transform.rotation * 
                Debug.Log("initialContactPoint: " + initialContactPoint.ToString());
                cuttee = collision.gameObject;
                hitCount++;
                thisCollider.isTrigger = true;

                //var newGO = new GameObject();
                //newGO.transform.position = transform.position;
                //newGO.transform.LookAt(initialContactPoint, transform.up);
                //newGO.transform.SetParent(cuttee.transform);
                //newGO.tag = "BladePosition";

                break;
            }
        }

        ToolUser1 tu = GetComponentInChildren<ToolUser1>();
        if (tu != null)
        {
            tu.transform.SetParent(null);
            tu.transform.position = initialContactPoint;
        }
    }

    int tempLength = 0;

    public void OnTriggerExit(Collider collider)
    {
        if (cuttee != null)
        {
			cutObject (cuttee);
            cuttee = null;
            hitCount--;
            thisCollider.isTrigger = false;
        }
    }

	void cutObject(GameObject cuttee){
		ToolUser1 tu = FindObjectOfType<ToolUser1>();
		string preCutTag = cuttee.tag;
        Vector3 cutterPosition = initialContactPoint; // - new Vector3(0, 1, 0);
        
        tu.transform.LookAt(transform.position, transform.up);

        //Transform[] bladePositions = cuttee.GetComponentsInChildren<Transform>();
        //foreach(Transform bladeTransform in bladePositions)
        //{
        //    if(bladeTransform.tag == "BladePosition")
        //    {
        //        tu.transform.position = bladeTransform.position;
        //        tu.transform.rotation = bladeTransform.rotation;
        //    }
        //}

        GameObject[] pieces = tu.cut(cuttee);

        if(pieces.Length == 1)
        {
            tu.transform.LookAt(cuttee.transform.position, transform.up);
            pieces = tu.cut(cuttee);
        }


        foreach (var p in pieces)
		{
            if (cuttee.tag == "Wood")
                p.tag = preCutTag;
            else if (cuttee.tag == "WoodCube" || cuttee.tag == "WoodCubeCut")
                p.tag = "WoodCubeCut";

            BoxCollider[] mcs = p.GetComponents<BoxCollider>();
			foreach(BoxCollider collider in mcs)
			{
                collider.isTrigger = true;
			}
            if (mcs.Length == 0)
            {
                BoxCollider bc = p.AddComponent<BoxCollider>();
                bc.isTrigger = true;
            }

            Rigidbody rb = p.GetComponent<Rigidbody>();
            if (rb == null) { 
                Rigidbody rbNew = p.AddComponent<Rigidbody>();
                rbNew.isKinematic = false;
            }
            else {
                rb.isKinematic = false;
            }
        }
        tu.transform.SetParent(this.transform);
        tu.transform.localPosition = new Vector3(0, 0, 0);
        tu.transform.localRotation = new Quaternion(0, 0, 0, 1);
    }
}
