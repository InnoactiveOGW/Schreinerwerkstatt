using UnityEngine;
using System.Collections;

public class WoodDivider : MonoBehaviour
{

    GameObject clone;
    bool canBeCloned = true;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }

    void OnTriggerEnter(Collider other)
    {
        Tool t = other.GetComponent<Tool>();
        if (t) {
            Debug.Log("tool used: " + t.ToString());
            t.doAction(this.gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
            // Find the impact point
            foreach (ContactPoint contact in collision.contacts)
            {
                Tool t = collision.gameObject.GetComponent<Tool>();

                if (contact.otherCollider == collision.collider && t != null && this.canBeCloned)
                {


                    Debug.Log("Tool hit wood");

                    //Impact(contact.point, collision.impactForceSum, m_ImpactShape, m_ImpactType);

                    // TODO
                    if (this.canBeCloned)
                    {
                        //this.canBeCloned = false;
                        StartCoroutine(toggleCloneability());
                        Debug.Log("instance ID: " + this.gameObject.GetInstanceID() + "canBeCloned: " + canBeCloned.ToString());
                        clone = GameObject.Instantiate(this.gameObject);
                        WoodDivider clonedWoodDivider = clone.GetComponent<WoodDivider>();
                        if (clonedWoodDivider != null)
                        {
                            StartCoroutine(clonedWoodDivider.toggleCloneability());
                            Debug.Log("instance ID: " + clonedWoodDivider.gameObject.GetInstanceID() + "canBeCloned: " + clonedWoodDivider.canBeCloned.ToString());
                        }
                    }

                    Vector3 originalPosition = this.gameObject.transform.position;
                    Vector3 originalScale = this.gameObject.transform.localScale;
                    Vector3 localContactPoint = originalPosition - contact.point;

                    // line: AB, Point P -> project P on AB
                    // A + dot(AP,AB) / dot(AB,AB) * AB
                    // A ist bei uns im lokalen Koordinatensystem der Punkt (0,0,0)

                    // falls der kürzer Teil des Holzstücks hinten liegt, wird das Holzstück so verschoben, dass er vorne liegt -> ausbessern!
                    Vector3 ab = gameObject.transform.forward;
                    Vector3 ap = localContactPoint;
                    Vector3 projectedConPoint = Vector3.Dot(ap, ab) / Vector3.Dot(ab, ab) * ab;
                    var distanceToCenter = projectedConPoint.magnitude;
                    // Math.Abs(newV3.z) / this.transform.localScale.z;

                    var scaleA1 = ((originalScale.z / 2) + distanceToCenter);
                    var scaleA2 = originalScale.z - scaleA1;

                    if (scaleA1 > originalScale.z || scaleA2 > originalScale.z)
                        break;

                    Vector3 newScale1 = new Vector3(originalScale.x, originalScale.y, scaleA1);
                    Vector3 newScale2 = new Vector3(originalScale.x, originalScale.y, scaleA2);
                    this.gameObject.transform.localScale = newScale1;
                    var posA2 = projectedConPoint + scaleA2 * gameObject.transform.forward / 2;

                    clone.transform.localScale = newScale2;

                    var posA1 = scaleA1 * gameObject.transform.forward / 2 - projectedConPoint;
                    clone.transform.position = originalPosition - posA2; // new Vector3(originalPosition.x, originalPosition.y, originalPosition.z - posA2);
                    this.gameObject.transform.position = originalPosition + posA1; //new Vector3(originalPosition.x, originalPosition.y, originalPosition.z + posA1);
                    break;


                }
            }
        }

    public IEnumerator toggleCloneability()
    {
        this.canBeCloned = false;
        Debug.Log("instance ID: " + this.gameObject.GetInstanceID() + "canBeCloned: " + canBeCloned.ToString());
        yield return new WaitForSeconds(3);
        this.canBeCloned = true;
        Debug.Log("instance ID: " + this.gameObject.GetInstanceID() + "canBeCloned: " + canBeCloned.ToString());
    }

}
