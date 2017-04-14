using UnityEngine;

public class destroyer : MonoBehaviour {


    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.other.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
