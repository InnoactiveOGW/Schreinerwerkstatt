using UnityEngine;
using System.Collections;

public class DestroyWood : MonoBehaviour {



    void OnTriggerEnter(Collider collider) {
        //GameObject collider = collision.gameObject;
        if (collider.tag == "WoodCubeCut")
        {
            CuttingGameController gameCtrl = FindObjectOfType<CuttingGameController>();
            gameCtrl.addScore(1);
            Destroy(collider.gameObject);
        }
        else if (collider.tag == "WoodCube") {
            CuttingGameController gameCtrl = FindObjectOfType<CuttingGameController>();
            gameCtrl.addMiss(1);
            Destroy(collider.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject collider = collision.gameObject;
        if (collider.tag == "WoodCubeCut")
        {
            CuttingGameController gameCtrl = FindObjectOfType<CuttingGameController>();
            gameCtrl.addScore(1);
            Destroy(collider.gameObject);
        }
        else if (collider.tag == "WoodCube")
        {
            CuttingGameController gameCtrl = FindObjectOfType<CuttingGameController>();
            gameCtrl.addMiss(1);
            Destroy(collider.gameObject);
        }
    }

}
