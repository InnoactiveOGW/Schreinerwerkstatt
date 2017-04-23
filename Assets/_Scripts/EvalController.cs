using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class EvalController : MonoBehaviour {


    [SerializeField]
    GameObject woodPrefab;

    GameObject currentWood;

    Dictionary<string, List<float>> accuracyValues;
    Dictionary<string, List<float>> timeValues;

    int numberOfTries;

    float start_time;
    float end_time;

    public void startTimer()
    {
        start_time = Time.time;
    }

    void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Wood")
        {
            handleWoodCut(collision.gameObject);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Wood")
        {
            handleWoodCut(collider.gameObject);
            destroyAllWoods();
            currentWood = Instantiate(woodPrefab);
        }
    }

    void destroyAllWoods()
    {
        GameObject[] woods = GameObject.FindGameObjectsWithTag("Wood");
        foreach (GameObject wood in woods)
        {
            Destroy(wood);
        }
    }

    void handleWoodCut(GameObject wood)
    {
        Debug.Log("handle wood cut");
        string interactionType = getInteractionType(wood);
        float accuracy = getAccuracy(wood);
        float time = getTime(wood);

        List<float> accuracyForName = getListFromDic(interactionType, 0);
        List<float> timeForName = getListFromDic(interactionType, 1);
        accuracyForName.Add(accuracy);
        timeForName.Add(time);
    }

    List<float> getListFromDic(string interactionType, int type)
    {
        Dictionary<string, List<float>> dic = type == 0 ? accuracyValues : timeValues;
        List <float> list = dic[interactionType];
        if (list == null)
        {
            list = new List<float>();
            accuracyValues.Add(interactionType, list);
        }
        return list;
    }

    string getInteractionType(GameObject wood)
    {
        GameController gc = FindObjectOfType<GameController>();
        return gc.getInteractionType();
    }

    float getAccuracy(GameObject wood)
    {
        return 0; // TODO
    }

    float getTime(GameObject wood)
    {
        end_time = Time.time;
        return end_time - start_time;
    }

    void spawnNewWood()
    {
        currentWood = Instantiate(woodPrefab);
    }

    void writeValuesToFile()
    {
        using (System.IO.StreamWriter w = File.AppendText("log.txt")){
            w.WriteLine("New user: "); // TODO: wie kommt der Nutzername in das Programm ?
            foreach(string interactionType in accuracyValues.Keys)
            {
                w.WriteLine("Interaction type: " + interactionType);
                w.WriteLine("Accuracy values: ");
                float accSum = 0;
                int counter = 0;
                foreach (float accuracyVal in accuracyValues[interactionType])
                {
                    accSum += accuracyVal;
                    w.Write(counter + ": " + accuracyVal + ", ");
                    counter++;
                }
                w.WriteLine("Number of accuracy values: " + accuracyValues[interactionType].Count);
                w.WriteLine("Accuracy average: " + accSum / accuracyValues[interactionType].Count);

                w.WriteLine("Time values: ");
                float timeSum = 0;
                counter = 0;
                foreach (float timeVal in timeValues[interactionType])
                {
                    timeSum += timeVal;
                    w.Write(counter + ": " + timeVal + ", ");
                    counter++;
                }
                w.WriteLine("Number of time values: " + timeValues[interactionType].Count);
                w.WriteLine("Time average: " + timeSum / timeValues[interactionType].Count);
                w.WriteLine("Number of Tests: " + numberOfTries);
            }
            w.Close();
        }
    }
}
