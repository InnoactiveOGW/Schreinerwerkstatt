using UnityEngine;
using System.Collections;

public class Divider : MonoBehaviour {

    public bool canBeDivided = true;
    public Vector3 initialContactPoint;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void divideGameObject() {
        // TODO
    }



    public void toggleDivideability()
    {
        StartCoroutine(runCoroutineForDivideability());
    }

    IEnumerator runCoroutineForDivideability() {
        this.canBeDivided = false;
        yield return new WaitForSeconds(3);
        this.canBeDivided = true;
    }

}
