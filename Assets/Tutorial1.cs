using UnityEngine;
using System.Collections;

public class Tutorial1 : MonoBehaviour {

	
    // Update is called once per frame
    float maxUpAndDown     = 0.1f;             // amount of meters going up and down
    float speed             = 200;            // up and down speed
   protected float angle        = 0;            // angle to determin the height by using the sinus
   protected float toDegrees     = Mathf.PI/180;    // radians to degrees
    Transform tran;
    float start;

    private void Start()
    {
        tran = this.transform;
        start = tran.position.y;
    }
    private void FixedUpdate()
    {
        angle += speed * Time.deltaTime;
        if (angle > 360) angle -= 360;
        this.transform.position = new Vector3(tran.position.x, start  +maxUpAndDown * Mathf.Sin(angle * toDegrees), tran.position.z);
 
    }
}
