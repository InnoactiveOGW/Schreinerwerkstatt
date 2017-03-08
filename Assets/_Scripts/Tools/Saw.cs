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
    public bool triggerEntered = false;
    Divider wood = null;
    GameObject oldParent = null;
    Vector3 oldPositionToParent;
    Quaternion oldRotationToParent;

    public float power = 0.0005f;
    public float movementDelay = 0.005f;

    float lastCutTimer;

    [SerializeField]
    float cutDelay;

    Transform parentTransform;

    //Audio
    AudioSource sawForward;
    AudioSource sawBack;

    // Use this for initialization
    void Start () {
        parentTransform = gameObject.transform.parent.transform;
        lastCutTimer = cutDelay;

        AudioSource[] audio = GetComponents<AudioSource>();
        if (audio.Length > 1)
        {
            sawForward = audio[0];
            sawBack = audio[1];
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        Pickup parent = parentTransform.GetComponent<Pickup>();
        if (isSawing && parent != null && parent.isPickedup) {
			Transform t = parentTransform;
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
                    float yForwForw = Vector3.Angle(normalizedVelocity, transform.forward);
                    if (yForwForw < 15)
                    {
                        t.position += transform.forward * yForwForw * power;
                    }
                    else
                    {
                        float yForwUp = Vector3.Dot(transform.up, controller.velocity);
                        t.position += transform.up * yForwUp * movementDelay;
                        t.position += transform.forward * Mathf.Abs(yForwUp) * -power;

                        //add condition for foorwad and backward
                        if (sawForward != null)
                            sawForward.Play();
                    }
                }
			}
		}
    }

    void Update()
    {
        Pickup parent = parentTransform.GetComponent<Pickup>();
        if (!isSawing && lastCutTimer < cutDelay)
        {
            lastCutTimer += Time.deltaTime;
            if(lastCutTimer >= cutDelay)
            {
                lastCutTimer = cutDelay;
            }
        }

        if(isSawing && parent.isPickedup && parentTransform.parent != null)
        {
            parentTransform.parent = null;
        }
    }

    bool objectCanBeCut(GameObject cuttee)
    {
        return oldParent == null && cuttee.tag == "Wood" && lastCutTimer >= cutDelay;
    }

    public void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.otherCollider == collision.collider && objectCanBeCut(collision.gameObject))
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
        if(collision.gameObject.tag == "Wood" && isSawing) // && !triggerEntered
        {
            float cutDirection = Vector3.Dot((initialContactPoint - transform.position).normalized, (transform.forward).normalized);
            if (cutDirection < 0.2 || !triggerEntered)
            {
                Debug.Log("Exited wood on the wrong side => no cut, cutDirection: " + cutDirection);
            }
            else
            {
                cutObject(cuttee, collision);
            }
            endSawingMode();
            cuttee = null;
        }
    }

	void cutObject(GameObject cuttee, Collision collision){
        lastCutTimer = 0;
        Material capMaterial = defaultCapMaterial;
		Vector3 tempScale = cuttee.transform.localScale;
		Vector3 tempPosition = cuttee.transform.position;
		ToolUser1 tu = FindObjectOfType<ToolUser1>();

		string preCutTag = cuttee.tag;
        Vector3 cutterPosition = initialContactPoint; // - new Vector3(0, 1, 0);

        // TODO: initialRotation should be oriented towards the nearest glue if the object has already been glue to another object


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
    }

    void setCutteRigidbody()
    {
        if (cuttee != null)
        {
            Rigidbody cutteRB = cuttee.GetComponent<Rigidbody>();
            if (cutteRB != null)
            {
                cutteRB.isKinematic = false;
            }
        }
    }

    void endSawingMode() {
        isSawing = false;
        
        setCutteRigidbody();

        Pickup pu = parentTransform.gameObject.GetComponent<Pickup>();
        if (pu != null)
        {
            pu.GetPickedUp(oldParent);
            pu.gameObject.transform.localPosition = oldPositionToParent;
            pu.gameObject.transform.localRotation = oldRotationToParent;
            
        }
        oldParent = null;
    }


    void enterSawingMode()
    {
        isSawing = true;
        oldParent = parentTransform.parent.gameObject;
        oldPositionToParent = parentTransform.localPosition;
        oldRotationToParent = parentTransform.localRotation;
        parentTransform.parent = null;

        //Pickup pu = parentTransform.gameObject.GetComponent<Pickup>();
        //if(pu != null)
        //{
        //    pu.gameObject.transform.parent = null;
        //}
        //Debug.Log("sawing mode entered");
    }
}