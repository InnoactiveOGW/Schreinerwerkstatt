using UnityEngine;
using System.Collections;
using System;

public class Measure : Tool
{
    GameObject myLine;
    public float sizeFactor = 1;
    Vector3 startPoint;
    TextMesh valueText;
    // Use this for initialization
    void Start()
    {
        valueText = this.GetComponentInChildren<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey("m"))
        {

            if (myLine == null)
            {
                Debug.Log("Pressed right click.");
                myLine = new GameObject();
                myLine.AddComponent<LineRenderer>();
                LineRenderer lr = myLine.GetComponent<LineRenderer>();
                lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
                lr.SetColors(Color.yellow, Color.yellow);
                lr.SetWidth(0.01f, 0.01f);
                startPoint = this.gameObject.transform.position;
                lr.SetPosition(0, startPoint);

            }
            Vector3 endpoint = this.gameObject.transform.position;
            valueText.text = calculateDistance(startPoint, endpoint);
            DrawLine(endpoint);
        }
        else
        {
            Destroy(myLine);
            valueText.text = "";
        }


    }

    private string calculateDistance(Vector3 startPoint, Vector3 endpoint)
    {
        return ((startPoint - endpoint).magnitude).ToString();
    }

    void DrawLine(Vector3 end)
    {
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.SetPosition(1, end);
    }
}
