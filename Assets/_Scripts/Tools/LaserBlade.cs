using UnityEngine;
using System.Collections;

public class LaserBlade : MonoBehaviour {

    Vector3 initialContactPoint;
    Vector3 initialContactPosition;
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
	void FixedUpdate () {

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Wood")
            return;

        Debug.Log ("LaserBlade Collision");

        foreach (ContactPoint contact in collision.contacts)
        {
            //wood = collision.gameObject.GetComponent<Divider>();
            if (contact.otherCollider == collision.collider)
            {
                initialContactPoint = contact.point;
                Vector3 obpos = this.gameObject.transform.position;
                initialContactPosition = obpos;
                initialRotation = this.gameObject.transform.rotation;
                initialRightVector = collision.gameObject.transform.rotation * this.gameObject.transform.right; // this.gameObject.transform.rotation * 
                Debug.Log("initialContactPoint: " + initialContactPoint.ToString());
                cuttee = collision.gameObject;
				Rigidbody rb = cuttee.GetComponent<Rigidbody> ();
				rb.isKinematic = true;
                return;
            }
        }
    }

    int tempLength = 0;

    public void OnCollisionExit(Collision collision)
    {
        if (cuttee != null)
        {
			cutObject (cuttee);
        }
    }

	void cutObject(GameObject cuttee){
		// Material capMaterial = defaultCapMaterial;
		// Vector3 tempScale = cuttee.transform.localScale;
		// Vector3 tempPosition = cuttee.transform.position;
		ToolUser1 tu = FindObjectOfType<ToolUser1>();

		string preCutTag = cuttee.tag;
        Vector3 cutterPosition = initialContactPoint; // - new Vector3(0, 1, 0);

		GameObject[] pieces = tu.cut(cutterPosition, initialRotation, cuttee);

		foreach (var p in pieces)
		{
			p.tag = preCutTag;
			// p.transform.localScale = tempScale;

			MeshCollider[] mcs = p.GetComponents<MeshCollider>();
			foreach(MeshCollider mc in mcs)
			{
				MeshFilter meshfilter = mc.GetComponent<MeshFilter>();
				Mesh mesh = meshfilter.mesh;
				mc.sharedMesh = mesh;
			}
			if (mcs.Length == 0)
			{
				MeshCollider newMC = p.AddComponent<MeshCollider>();
				newMC.convex = true;
				newMC.sharedMesh = p.GetComponent<MeshFilter>().mesh;

				MeshCollider triggerMeshCollider = p.AddComponent<MeshCollider>();
				triggerMeshCollider.convex = true;
				triggerMeshCollider.isTrigger = true;
			}

			Rigidbody rb = p.GetComponent<Rigidbody>();
			if (rb == null)
				p.AddComponent<Rigidbody> ();
			else {
				rb.isKinematic = false;
			}
			Pickup pickup = p.GetComponent<Pickup>();
			if (pickup == null)
				p.AddComponent<Pickup>();
		}
    }
    
}
