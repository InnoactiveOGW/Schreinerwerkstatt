using UnityEngine;
using System.Collections;

public class WoodDivider : Divider
{

    GameObject clone;
    Tool tool;
    //Vector3 initialContactPoint;
    
    // Use this for initialization
    void Start() {}

    // Update is called once per frame
    void Update() {}

    public void OnCollisionExit(Collision collision) {
        bool isValidCut = true;
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.otherCollider == collision.collider)
            {
                Vector3 localContactPoint = transform.position - initialContactPoint;

                // TODO
                isValidCut = true;
                break;

                // TODO
                Vector3 testVec1 = Vector3.Cross(localContactPoint, transform.up);
                Vector3 testVec2 = Vector3.Cross(contact.point, transform.up);
                float x1 = Mathf.Sign(testVec1.x); float y1 = Mathf.Sign(testVec1.y); float z1 = Mathf.Sign(testVec1.z);
                float x2 = Mathf.Sign(testVec1.x); float y2 = Mathf.Sign(testVec1.y); float z2 = Mathf.Sign(testVec1.z);
                if (x1 == x2 && y1 == y2 && z1 == z2) {

                }

            }
        }
            
        if(tool != null && collision.gameObject == tool.gameObject) { 
            if(isValidCut)
                divideGameObject();
            this.tool = null;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
            // Find the impact point
            foreach (ContactPoint contact in collision.contacts)
            {
                Tool t = collision.gameObject.GetComponent<Tool>();
                if (contact.otherCollider == collision.collider && t != null)
                {
                    initialContactPoint = contact.point;
                    this.tool = t;
                    return;
                }


            }
        }

    public override void divideGameObject() {
        if (!canBeDivided)
            return;
        Debug.Log("divide game object");
        Vector3 originalPosition = this.gameObject.transform.position;
        Vector3 originalScale = this.gameObject.transform.localScale;
        Vector3 localContactPoint = originalPosition - initialContactPoint;

        // line: AB, Point P -> project P on AB
        // A + dot(AP,AB) / dot(AB,AB) * AB
        // A ist bei uns im lokalen Koordinatensystem der Punkt (0,0,0)

        Vector3 ab = gameObject.transform.forward;
        Vector3 ap = localContactPoint;
        Vector3 projectedConPoint = Vector3.Dot(ap, ab) / Vector3.Dot(ab, ab) * ab;
        float distanceToCenter = projectedConPoint.z;

        float scaleA1 = ((originalScale.z / 2) + distanceToCenter);
        var scaleA2 = originalScale.z - scaleA1;

        if (scaleA1 > originalScale.z || scaleA2 > originalScale.z)
        {
            Debug.Log("scaleA1: " + scaleA1 + " ,scaleA2: " + scaleA2);
            Debug.Log("originalScale.z: " + originalScale.z + " ,originalScale.z: " + originalScale.z);
            return;
        }
            

        if (canBeDivided)
        {
            Debug.Log("game object canBeDivided");
            toggleDivideability();
            clone = GameObject.Instantiate(this.gameObject);
            WoodDivider clonedWoodDivider = clone.GetComponent<WoodDivider>();
            if (clonedWoodDivider != null)
            {
                clonedWoodDivider.toggleDivideability();
            }
        }
        
        Vector3 newScale1 = new Vector3(originalScale.x, originalScale.y, scaleA1);
        
        this.gameObject.transform.localScale = newScale1;

        var posA1 = scaleA1 * gameObject.transform.forward / 2 - projectedConPoint;
        this.gameObject.transform.position = originalPosition + posA1; //new Vector3(originalPosition.x, originalPosition.y, originalPosition.z + posA1);

        Vector3 newScale2 = new Vector3(originalScale.x, originalScale.y, scaleA2);
        clone.transform.localScale = newScale2;

        var posA2 = projectedConPoint + scaleA2 * gameObject.transform.forward / 2 * 1.05f;
        clone.transform.position = originalPosition - posA2; // new Vector3(originalPosition.x, originalPosition.y, originalPosition.z - posA2);
        Debug.Log("game object divsion finished");
    }
}
