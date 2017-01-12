using UnityEngine;
using System.Collections;
using System;

public class Measure : Tool
{
    GameObject myLine;

    Vector3 startPoint;
    TextMesh valueText;
    // Use this for initialization
    void Start()
    {
        valueText = this.GetComponentInChildren<TextMesh>();
        isPickedup = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey("g") && isPickedup)
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
        Debug.Log(Config.sizeFactor);
        return (((startPoint - endpoint).magnitude) * Config.sizeFactor).ToString();

    }

    void DrawLine(Vector3 end)
    {
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.SetPosition(1, end);
    }
}
