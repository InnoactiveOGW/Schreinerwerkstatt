using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Wood : MonoBehaviour {

    public float time = 0;
    public float accuracy = 0;
    public string interactionType = "";

    float start_time;
    float end_time;

    [SerializeField]
    GameObject sawGizmo;

    List<Pickup> watchedPickups;

    bool activateMarkers = false;
    bool checkMarkers = false;
    GameObject saw;

    MagneticMarker[] markers;
    MagneticMarker currentMarker;

    public Material normalMaterial;
    public Material highlightMaterial;

    void Start()
    {
        start_time = Time.time;
        currentMarker = null;
        sawGizmo = GameObject.Find("SawGizmo");
        markers = GetComponentsInChildren<MagneticMarker>();
        switchMarkers(false);
        activateMarkers = false;

        watchedPickups = GetWatchedPickups();
    }

    private List<Pickup> GetWatchedPickups()
    {
        List<Pickup> resultList = new List<Pickup>();
        Saw[] saws = FindObjectsOfType<Saw>();
        foreach(Saw saw in saws)
        {
            if (saw.snapToPoint)
            {
                Pickup toAdd = saw.GetComponentInParent<Pickup>();
                resultList.Add(toAdd);
            }
        }
        return resultList;
    }

    public void evalCut(float newAcc, string newInteractionType)
    {
        end_time = Time.time;
        time = end_time - start_time;
        accuracy = newAcc;
        interactionType = newInteractionType;
        EvalController.Instance.evalWoodCut(this);
    }

    void handleMarkers()
    {
        bool temp = false;
        foreach (Pickup pu in watchedPickups)
        {
            if (pu.isPickedup)
            {
                temp = true;
                Debug.Log("temp is true");
            }
        }

        if(activateMarkers != temp)
        {
            Debug.Log("(activateMarkers != temp");
            activateMarkers = temp;
            switchMarkers(activateMarkers);
        }
    }

    void switchMarkers(bool newSwitch)
    {
        foreach(MagneticMarker marker in markers)
        {
            marker.gameObject.SetActive(newSwitch);
        }
    }

    Saw GetSaw()
    {
        Saw[] saws = FindObjectsOfType<Saw>();
        foreach(Saw saw in saws)
        {
            Pickup pickup = saw.GetComponentInParent<Pickup>();
            if(pickup != null && pickup.isPickedup)
            {
                return saw;
            }
        }
        return null;
    }

    void Update()
    {
        handleMarkers();
        Saw s = GetSaw();
        if(s != null)
            saw = s.gameObject;

        if (checkMarkers && saw != null)
        {
            Vector3 sawPosition = saw.transform.position;
            MagneticMarker closestMarker = null;
            float minDistance = -1;
            foreach (MagneticMarker marker in markers)
            {
                float currentDistance = (sawPosition - marker.transform.position).sqrMagnitude;
                if (minDistance == -1 || minDistance > currentDistance)
                {
                    minDistance = currentDistance;
                    closestMarker = marker;
                }
            }
            if(currentMarker != null)
            {
                deactivateMarker(currentMarker);
            }
            currentMarker = closestMarker;
            activateMarker(currentMarker);
        }
    }
    
    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "saw" && activateMarkers)
        {
            checkMarkers = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "saw" && activateMarkers)
        {
            checkMarkers = false;
        }
    }

    private void activateMarker(MagneticMarker marker)
    {
        marker.currentRenderer.material = highlightMaterial;
    }

    private void deactivateMarker(MagneticMarker marker)
    {
        marker.currentRenderer.material = normalMaterial;
    }
}
