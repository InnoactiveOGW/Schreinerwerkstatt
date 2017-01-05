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

    // Use this for initialization
    void Start () {
        parentTransform = gameObject.transform.parent.transform;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        Transform t = gameObject.transform.parent.transform;
        if (Input.GetKey("r") && isSawing)
        {
            t.position += transform.forward * 0.01f;

        }
        else if (Input.GetKey("f") && isSawing)
        {

            t.position -= transform.forward * 0.01f;
            t.position -= transform.up * 0.005f;

        }

        if (isSawing && false)
        {
            SteamVR_TrackedObject inputDevice = oldParent.GetComponent<SteamVR_TrackedObject>();
            SteamVR_Controller.Device controller = SteamVR_Controller.Input((int)inputDevice.index);

            // if (controller != null && Mathf.Abs(controller.velocity.z) > 0.3)
            if (controller != null)
            {
                Vector3 normalizedVelocity = controller.velocity;
                transform.position += transform.forward * Vector3.Dot(transform.forward, controller.velocity) * movementDelay;
                transform.position +=  transform.up * Mathf.Abs(Vector3.Dot(transform.forward, controller.velocity)) * -power;
                //Debug.Log("controller.velocity: " + controller.velocity.ToString());
                //Debug.Log("normalizedVelocityy: " + normalizedVelocity.ToString());
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected");
        foreach (ContactPoint contact in collision.contacts)
        {
            //wood = collision.gameObject.GetComponent<Divider>();
            if (contact.otherCollider == collision.collider && oldParent == null && collision.gameObject.tag == "Wood")
            {
                initialContactPoint = contact.point;
                Vector3 obpos = this.gameObject.transform.position;
                initialContactPosition = obpos;
                initialRotation = this.gameObject.transform.rotation;
                initialRightVector = collision.gameObject.transform.rotation * this.gameObject.transform.right; // this.gameObject.transform.rotation * 
                Debug.Log("initialContactPoint: " + initialContactPoint.ToString());
                cuttee = collision.gameObject;
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
                MeshCollider mc = p.GetComponent<MeshCollider>();
                if (mc == null) {
                    mc = p.AddComponent<MeshCollider>();
                    mc.convex = true;
                }
                mc.sharedMesh = p.GetComponent<MeshFilter>().mesh;
                MeshCollider triggerMeshCollider = p.AddComponent<MeshCollider>();
                triggerMeshCollider.convex = true;
                triggerMeshCollider.isTrigger = true;
                Rigidbody rb = p.GetComponent<Rigidbody>();
                if(rb == null)
                    p.AddComponent<Rigidbody>();
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

    }

    void enterSawingMode()
    {
        isSawing = true;
        oldParent = parentTransform.parent.gameObject;
        Pickup pu = parentTransform.gameObject.GetComponent<Pickup>();
        if(pu != null)
        {
            pu.GetReleased();
        }
        Debug.Log("sawing mode entered");
    }


    
}
