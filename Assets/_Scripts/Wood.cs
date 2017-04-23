using UnityEngine;
using System.Collections;

public class Wood : MonoBehaviour {

    public float time = 0;
    public float accuracy = 0;
    public string interactionType = "";

    float start_time;
    float end_time;
    
    void Start()
    {
        start_time = Time.time;
    }

    public void evalCut(float newAcc, string newInteractionType)
    {
        end_time = Time.time;

        time = end_time - start_time;

        accuracy = newAcc;

        interactionType = newInteractionType;


        EvalController.Instance.evalWoodCut(this);
    }
}
