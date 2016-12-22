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
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetKey("r") && isSawing)
        {

            transform.position += transform.forward * 0.01f;

        }
        else if (Input.GetKey("f") && isSawing)
        {

            transform.position -= transform.forward * 0.01f;
            transform.position -= transform.up * 0.005f;

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
            //GetPickedUp(oldParent);
            gameObject.transform.SetParent(null);
            isSawing = false;
            //wood = null;
            //oldParent = null;

            Material capMaterial = defaultCapMaterial;
            // TODO
            // welches Material wird hier verwendet?
            Vector3 tempScale = cuttee.transform.localScale;
            Vector3 tempPosition = cuttee.transform.position;
            ToolUser1 tu = FindObjectOfType<ToolUser1>();

            Vector3 cutterPosition = initialContactPoint - new Vector3(0, 1, 0);
            GameObject[] pieces = tu.cut(cutterPosition, initialRotation, cuttee);
            foreach (var p in pieces)
            {
                p.transform.localScale = tempScale;
                MeshCollider mc = p.GetComponent<MeshCollider>();
                if (mc == null)
                    mc = p.AddComponent<MeshCollider>();
                mc.sharedMesh = p.GetComponent<MeshFilter>().mesh;
            }
        }

    }

    void enterSawingMode()
    {
        Debug.Log("sawing mode entered");
        isSawing = true;
        oldParent = gameObject.transform.parent.gameObject;
        this.gameObject.transform.parent.SetParent(null);
        //GetReleased();
        //gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.01f, gameObject.transform.position.z);
    }


    
}
