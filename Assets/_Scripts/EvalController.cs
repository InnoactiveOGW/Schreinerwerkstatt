using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine.UI;

public class EvalController : UnitySingleton<EvalController> {

    [SerializeField]
    GameObject woodPrefab;

    GameObject currentWood;

    Dictionary<string, List<float>> accuracyValues;
    Dictionary<string, List<float>> timeValues;
    Dictionary<string, List<float>> dividerValues;

    int numberOfTries = 0;

    [SerializeField]
    string playerName;

    [SerializeField]
    TextMesh taskText;

    [SerializeField]
    ParticleSystem particle;

    [SerializeField]
    GameObject hand;

    [SerializeField]
    List<Pickup> saws;

    Pickup currentSaw;
    int sawCounter = 0;

    float start_time = 0;

    [SerializeField]
    float maxTimePerStage = 60;

    [SerializeField]
    int maxNumberOfCuts = 2;

    bool readyToSwitchSaw = false;
    bool startedStudy = false;

    float currentDivider;



    void Start()
    {
        accuracyValues = new Dictionary<string, List<float>>();
        timeValues = new Dictionary<string, List<float>>();
        dividerValues = new Dictionary<string, List<float>>();
    }

    //void Update()
    //{
    //    float current_time = Time.time;
    //    if(current_time - start_time > maxTimePerStage)
    //    {
    //        // TODO
    //        switchSaw();
    //    }
    //}

    void switchSaw()
    {
        sawCounter++;
        Vector3 currentSawLocalPosition = currentSaw.transform.position;
        Quaternion currentSawLocalRotation = currentSaw.transform.rotation;
        currentSaw.GetReleased();
        currentSaw.Reset();
        if (sawCounter < saws.Count)
        {
            currentSaw = saws[sawCounter];
            currentSaw.GetPickedUp(hand, true);
            //currentSaw.transform.localPosition = currentSawLocalPosition;
            //currentSaw.transform.localRotation = currentSawLocalRotation;
        }
        else
        {
            // TODO
            writeValuesToFile();
        }
    }

    public void startStudy(GameObject hand)
    {
        this.hand = hand;
        Debug.Log("starting study");

        start_time = Time.time;
        foreach(Pickup saw in saws)
        {
            saw.GetReleased();
        }
        currentSaw = saws[sawCounter];
        currentSaw.GetPickedUp(this.hand, true);
        startedStudy = true;
        numberOfTries = 0;

        currentDivider = GetNewRandomDivider();
        taskText.text = currentDivider.ToString("n2");
    }

    private float GetNewRandomDivider()
    {
        return UnityEngine.Random.Range(0.15f, 0.85f);
    }

    void OnCollisionEnter(Collision collision)
    {
        handleWoodCut(collision.gameObject);
    }

    void OnTriggerEnter(Collider collider)
    {
        handleWoodCut(collider.gameObject);
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
        if (wood.tag == "Wood")
        {
            destroyAllWoods();
            particle.Play();
            currentWood = Instantiate(woodPrefab);
            if (startedStudy)
            {
                if (numberOfTries >= maxNumberOfCuts)
                {
                    numberOfTries = 0;
                    switchSaw();
                }
                currentDivider = GetNewRandomDivider();
                taskText.text = currentDivider.ToString("n2");
            }
            else
            {
                taskText.text = "";
            }
        }
    }

    public void evalWoodCut(Wood wood, Saw saw)
    {
        numberOfTries++;
        Debug.Log("handle wood cut");
        string interactionType = getInteractionType(wood);
        float accuracy = getAccuracy(wood);
        float time = getTime(wood);

        List<float> accuracyForName = getListFromDic(interactionType, accuracyValues);
        List<float> timeForName = getListFromDic(interactionType, timeValues);
        accuracyForName.Add(accuracy);
        timeForName.Add(time);

        Debug.Log("Number of tries: " + numberOfTries + ", accuracy: " + accuracy + ", time: " + time);

        Vector3 vecBetweenWoodCenterAndSaw = wood.transform.position - saw.transform.position;
        float distance = vecBetweenWoodCenterAndSaw.magnitude;
        float error = Mathf.Abs(currentDivider - distance);

        List <float> dividerForName = getListFromDic(interactionType, dividerValues);
        dividerForName.Add(error);

    }

    List<float> getListFromDic(string interactionType, Dictionary<string, List<float>> dic)
    {
        List<float> list = null;
        bool interactionTypeListExists = dic.TryGetValue(interactionType, out list);
        if (!interactionTypeListExists)
        {
            list = new List<float>();
            dic.Add(interactionType, list);
        }
        return list;
    }

    string getInteractionType(Wood wood)
    {
        return wood.interactionType;
    }

    float getAccuracy(Wood wood)
    {
        return wood.accuracy;
    }

    float getTime(Wood wood)
    {
        return wood.time;
    }

    void writeValuesToFile()
    {
        using (System.IO.StreamWriter w = File.AppendText("log.txt")){
            w.WriteLine("New user: " + playerName);
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
                w.WriteLine("");
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
                w.WriteLine("");
                w.WriteLine("Error values: ");
                float errorSum = 0;
                counter = 0;
                foreach (float errorVal in dividerValues[interactionType])
                {
                    errorSum += errorVal;
                    w.Write(counter + ": " + errorVal + ", ");
                    counter++;
                }
                w.WriteLine("Number of error values: " + dividerValues[interactionType].Count);
                w.WriteLine("Error average: " + errorSum / dividerValues[interactionType].Count);
                w.WriteLine("---");
            }

            w.WriteLine("Number of cuts: " + maxNumberOfCuts * accuracyValues.Keys.Count);

            float end_time = Time.time;
            w.WriteLine("Full time: " + (end_time - start_time));
            w.Close();
        }
    }
}
