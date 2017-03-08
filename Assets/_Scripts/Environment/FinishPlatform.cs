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
        finish(collider.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        finish(collision.gameObject);
    }

    void finish(GameObject finishedObj)
    {
        EvaluationCtrl evalC = finishedObj.GetComponent<EvaluationCtrl>();
        if (evalC != null)
        {
            scoreText.text = evalC.rate().ToString("F1");
            particle.Play();
            Destroy(evalC.gameObject);
        }
    }
}
