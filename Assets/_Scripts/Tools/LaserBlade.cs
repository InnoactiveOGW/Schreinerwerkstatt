using UnityEngine;
using System.Collections;

public class LaserBlade : MonoBehaviour {

    Vector3 initialContactPoint;
    Vector3 initialContactPosition;
    Vector3 initialUpVector;
    Quaternion initialRotation;
    Vector3 initialRightVector;
    GameObject cuttee;
    public Material defaultCapMaterial;
    GameObject blade;

    bool isSawing = false;
    Divider wood = null;
    GameObject oldParent = null;
    //ToolUser blade;
    //new Rigidbody rigidbody;
    public float power = 0.0005f;
    public float movementDelay = 0.005f;

    public Transform parentTransform;

    int hitCount = 0;

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
    }
	
	// Update is called once per frame
	void Update () {
        if (hitCount > 0 && cuttee == null) { 
            hitCount = 0;
            CapsuleCollider thiscollider = GetComponent<CapsuleCollider>();
            thiscollider.isTrigger = false;
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
            //wood = collision.gameObject.GetComponent<Divider>();
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
                //Rigidbody rb = cuttee.GetComponent<Rigidbody>();
                //rb.isKinematic = true;

                CapsuleCollider collider = GetComponent<CapsuleCollider>();
                collider.isTrigger = true;

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
            CapsuleCollider thiscollider = GetComponent<CapsuleCollider>();
            thiscollider.isTrigger = false;
        }
    }

	void cutObject(GameObject cuttee){
		ToolUser1 tu = FindObjectOfType<ToolUser1>();
		string preCutTag = cuttee.tag;
        Vector3 cutterPosition = initialContactPoint; // - new Vector3(0, 1, 0);
        tu.transform.LookAt(transform.position, transform.up);
        GameObject[] pieces = tu.cut(cuttee);

        foreach (var p in pieces)
		{
            if (cuttee.tag == "Wood")
                p.tag = preCutTag;
            else if (cuttee.tag == "WoodCube" || cuttee.tag == "WoodCubeCut")
                p.tag = "WoodCubeCut";

            BoxCollider[] mcs = p.GetComponents<BoxCollider>();
			foreach(BoxCollider collider in mcs)
			{
                //Destroy(mc);
                collider.isTrigger = true;
				//MeshFilter meshfilter = mc.GetComponent<MeshFilter>();
				//Mesh mesh = meshfilter.mesh;
				//mc.sharedMesh = mesh;
			}
            if (mcs.Length == 0)
            {
                BoxCollider bc = p.AddComponent<BoxCollider>();
                bc.isTrigger = true;

                //	MeshCollider newMC = p.AddComponent<MeshCollider>();
                //	newMC.convex = true;
                //	newMC.sharedMesh = p.GetComponent<MeshFilter>().mesh;

                //	MeshCollider triggerMeshCollider = p.AddComponent<MeshCollider>();
                //	triggerMeshCollider.convex = true;
                //	triggerMeshCollider.isTrigger = true;


            }



            Rigidbody rb = p.GetComponent<Rigidbody>();
            //if (rb != null)
            //    Destroy(rb);
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
