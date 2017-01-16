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
    AudioSource sawForward;
    AudioSource sawBack;

    bool isSawing = false;
    Divider wood = null;
    GameObject oldParent = null;
    //ToolUser blade;
    //new Rigidbody rigidbody;
    public float power = 0.0005f;
    public float movementDelay = 0.005f;

    public Transform parentTransform;

    // Use this for initialization
    void Start () {
        parentTransform = gameObject.transform.parent.transform;

        AudioSource[] audio = GetComponents<AudioSource>();
        if(audio.Length>1)
        {
            sawForward = audio[0];
            sawBack = audio[1];
        }
        

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetKeyDown("f"))
        {
            sawForward.Play();
        }
        if (Input.GetKeyDown("r"))
        {
            sawForward.Play();
        }


        if (isSawing) {
			Transform t = gameObject.transform.parent.transform;
			if (!Config.isVR) {
				if (Input.GetKey ("r")) {
					t.position += transform.forward * 0.01f;
                    sawForward.Play();

                } else if (Input.GetKey ("f")) {
					t.position -= transform.forward * 0.01f;
					t.position -= transform.up * 0.005f;
                    sawBack.Play();
				}
			} else {
				SteamVR_TrackedObject inputDevice = oldParent.GetComponentInParent<SteamVR_TrackedObject> ();
				SteamVR_Controller.Device controller = SteamVR_Controller.Input ((int)inputDevice.index);

				// if (controller != null && Mathf.Abs(controller.velocity.z) > 0.3)
				if (controller != null) {
					Vector3 normalizedVelocity = controller.velocity;
					t.position += t.forward * Vector3.Dot (t.forward, controller.velocity) * movementDelay;
					t.position += t.up * Mathf.Abs (Vector3.Dot (t.forward, controller.velocity)) * -power;
					//Debug.Log("controller.velocity: " + controller.velocity.ToString());
					//Debug.Log("normalizedVelocityy: " + normalizedVelocity.ToString());
				}
			}
		}
    }

    public void OnCollisionEnter(Collision collision)
    {
        // TODO: Bewertung: Berechnung wie weit der Kollisionspunkt vom gewünschten entfernt ist
        // -> TODO: Berechnung

        /* TODO: Wo liegt der gewünschte Schnittpunkt?
         * -> center = 0,0,0 & scale = 1,1,1 =>
         *  -> p = x,y,z
         *  -> p auf Linie (center, forwardVec) projezieren
         *  => p.z = Entfernung zum Mittelpunkt
         *  => scaleA = 0.33, scaleB = 0.33, scaleC = 0.33
         *  => s.z = scaleA / 2 || -scaleA / 2
         *  
         *  nächster Schnitt:
         *  s.z = 0
         */
        /*
            Vector3 ab = gameObject.transform.forward;
            Vector3 ap = localContactPoint;
            Vector3 projectedConPoint = Vector3.Dot(ap, ab) / Vector3.Dot(ab, ab) * ab;
            float distanceToCenter = projectedConPoint.z; 

        */
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

    public void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Wood")
            Debug.Log("collision exit");
        // && (initialContactPoint.y - collision.contacts[0].point.y) > 0.5
        if (isSawing && collision.gameObject.tag == "Wood")
        {
			cutObject (cuttee, collision);
        }

    }

	void cutObject(GameObject cuttee, Collision collision){
		Debug.Log("cutting");
		// gameObject.transform.SetParent(oldParent.transform);
		isSawing = false;
		//wood = null;

		Material capMaterial = defaultCapMaterial;
		// TODO
		// welches Material wird hier verwendet?
		Vector3 tempScale = cuttee.transform.localScale;
		Vector3 tempPosition = cuttee.transform.position;
		ToolUser1 tu = FindObjectOfType<ToolUser1>();

		string preCutTag = cuttee.tag;
		Vector3 cutterPosition = initialContactPoint - new Vector3(0, 1, 0);
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

		Pickup pu = parentTransform.gameObject.GetComponent<Pickup>();
		if(pu != null)
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
