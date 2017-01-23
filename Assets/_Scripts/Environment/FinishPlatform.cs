using UnityEngine;
using System.Collections;

public class FinishPlatform : MonoBehaviour {

    public GameObject explosion;
     ParticleSystem particle;
    private void Start()
    {
        particle =  explosion.GetComponent<ParticleSystem>();
    }
    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Wood")
        {
            GameController gc = FindObjectOfType<GameController>();
            gc.finishLevel(collider.gameObject);
            particle.Play();
            Destroy(collider.gameObject);
        }
    }
}
