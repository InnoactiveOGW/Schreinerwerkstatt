using UnityEngine;
using System.Collections;

public class FinishPlatform : MonoBehaviour {

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Wood")
        {
            GameController gc = FindObjectOfType<GameController>();
            gc.finishLevel(collider.gameObject);
        }
    }
}
