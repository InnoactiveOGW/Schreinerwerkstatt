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
        if(collider.gameObject.tag == "Wood")
        {
            GameController gc = FindObjectOfType<GameController>();
            scoreText.text =  gc.finishLevel(collider.gameObject);
            particle.Play();
            Destroy(collider.gameObject);
        }
    }
}
