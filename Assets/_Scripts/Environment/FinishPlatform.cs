using UnityEngine;
using System.Collections;

public class FinishPlatform : MonoBehaviour {

    public GameObject explosion;
     ParticleSystem particle;
    public GameObject Score;
    TextMesh scoreText;
    private void Start()
    {
        scoreText = Score.GetComponent<TextMesh>();
        particle =  explosion.GetComponent<ParticleSystem>();
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Wood")
        {
            finish(collider.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wood")
        {
            finish(collision.gameObject);
        }
    }

    void finish(GameObject finishedObj)
    {
        particle.Play();
        Destroy(finishedObj);
    }
}
