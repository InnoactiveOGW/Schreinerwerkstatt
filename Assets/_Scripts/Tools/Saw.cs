using UnityEngine;
using System.Collections;

public class Saw : MonoBehaviour {

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
		if (isSawing) {
			Transform t = gameObject.transform.parent.transform;
			if (!Config.isVR) {
				if (Input.GetKey ("r")) {
                    sawForward.Play();
                    t.position += transform.forward * 0.01f;
				} else if (Input.GetKey ("f")) {
                    sawForward.Play();
                    t.position -= transform.forward * 0.01f;
					t.position -= transform.up * 0.005f;
				}
			} else {
				SteamVR_TrackedObject inputDevice = oldParent.GetComponentInParent<SteamVR_TrackedObject> ();
				SteamVR_Controller.Device controller = SteamVR_Controller.Input ((int)inputDevice.index);

				// if (controller != null && Mathf.Abs(controller.velocity.z) > 0.3)
				if (controller != null) {
					Vector3 normalizedVelocity = controller.velocity;
					t.position += t.forward * Vector3.Dot (t.forward, controller.velocity) * movementDelay;
					t.position += t.up * Mathf.Abs (Vector3.Dot (t.forward, controller.velocity)) * -power;
                
                    //addcondition for foorwad and backward
                    if(sawForward != null)
                        sawForward.Play();
                }
			}
		}
    }

    // deprecated!!!
    // Bewertung: Berechnung wie weit der Kollisionspunkt vom gewünschten entfernt ist
    //
    /* Wo liegt der gewünschte Schnittpunkt?
     * -> center = 0,0,0 & scale = 1,1,1 =>
     *  -> p = x,y,z
     *  -> p auf Linie (center, forwardVec) projezieren
     *  => p.z = Entfernung zum Mittelpunkt
     *  => scaleA = 0.33, scaleB = 0.33, scaleC = 0.33
     *  => s.z = scaleA / 2 || -scaleA / 2
     *  
     *  nächster Schnitt:
     *  s.z = 0
     *  
     *  
        Vector3 ab = gameObject.transform.forward;
        Vector3 ap = localContactPoint;
        Vector3 projectedConPoint = Vector3.Dot(ap, ab) / Vector3.Dot(ab, ab) * ab;
        float distanceToCenter = projectedConPoint.z; 

    */

    public void OnCollisionEnter(Collision collision)
    {
		Debug.Log ("Collision");

        foreach (ContactPoint contact in collision.contacts)
        {
            //wood = collision.gameObject.GetComponent<Divider>();
            if (contact.otherCollider == collision.collider && oldParent == null && collision.gameObject.tag == "Wood")
            {
                ControllerCube cc = gameObject.transform.parent.GetComponentInParent<ControllerCube>();
				if (cc == null) {
					Debug.Log ("could not find Your Hand i want to saw dude");
					return;
				}

                if (Vector3.Dot((contact.point - transform.position).normalized, -transform.forward) < 0) {
                    Debug.Log("Hit wood at the wrong side => no sawing mode");
                    return;
                }
					
                initialContactPoint = contact.point;
                Vector3 obpos = this.gameObject.transform.position;
                initialContactPosition = obpos;
                initialRotation = this.gameObject.transform.rotation;
                initialRightVector = collision.gameObject.transform.rotation * this.gameObject.transform.right; // this.gameObject.transform.rotation * 
                Debug.Log("initialContactPoint: " + initialContactPoint.ToString());
                cuttee = collision.gameObject;
				Rigidbody rb = cuttee.GetComponent<Rigidbody> ();
				rb.isKinematic = true;
				enterSawingMode();
            }
        }
    }

    int tempLength = 0;

    public void OnCollisionStay(Collision collisionInfo)
    {
        foreach (ContactPoint contact in collisionInfo.contacts)
        {
            // Debug.DrawRay(contact.point, contact.normal * 10, Color.white);
            if (isSawing) {
                Debug.DrawRay(contact.point, contact.normal * 10, Color.white);
            }
        }
        if(isSawing && tempLength != collisionInfo.contacts.Length) { 
            Debug.Log(collisionInfo.contacts.Length);
            tempLength = collisionInfo.contacts.Length;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Wood")
            Debug.Log("collision exit");
        // && (initialContactPoint.y - collision.contacts[0].point.y) > 0.5

        if (Vector3.Dot((initialContactPoint - transform.position).normalized, (transform.forward).normalized) < 0) {
            Debug.Log("Exited wood on the wrong side => no cut");
            return;
        }

        if (isSawing && collision.gameObject.tag == "Wood")
        {
			cutObject (cuttee, collision);
        }

    }

	void cutObject(GameObject cuttee, Collision collision){
		Material capMaterial = defaultCapMaterial;
		Vector3 tempScale = cuttee.transform.localScale;
		Vector3 tempPosition = cuttee.transform.position;
		ToolUser1 tu = FindObjectOfType<ToolUser1>();

		string preCutTag = cuttee.tag;
        Vector3 cutterPosition = initialContactPoint; // - new Vector3(0, 1, 0);
		GameObject[] pieces = tu.cut(cutterPosition, initialRotation, cuttee);
		foreach (var p in pieces)
		{
			p.tag = preCutTag;
			p.transform.localScale = tempScale;

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

        endSawingMode();
    }

    void endSawingMode() {
        isSawing = false;
        Pickup pu = parentTransform.gameObject.GetComponent<Pickup>();
        if (pu != null)
        {
            pu.GetPickedUp(oldParent);
            pu.gameObject.transform.position = oldParent.transform.position;
            pu.gameObject.transform.rotation = oldParent.transform.rotation;
        }
        oldParent = null;
    }


    void enterSawingMode()
    {
        isSawing = true;
        oldParent = parentTransform.parent.gameObject;
        Pickup pu = parentTransform.gameObject.GetComponent<Pickup>();
        if(pu != null)
        {
            pu.gameObject.transform.parent = null;
            // pu.GetReleased();
        }
        Debug.Log("sawing mode entered");
    }


    
}
