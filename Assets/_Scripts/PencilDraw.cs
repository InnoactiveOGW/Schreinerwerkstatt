using UnityEngine;
using System.Collections;
using System;

public class PencilDraw : Tool
{
    public Camera sceneCamera, canvasCam;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider colider)
    {
        if (colider.gameObject.tag != "Wood") return;


        RaycastHit hit;
        if (!Physics.Raycast(this.gameObject.transform.position,
            -this.gameObject.transform.forward,
            out hit,
            float.PositiveInfinity,
            LayerMask.GetMask("Default"),
            QueryTriggerInteraction.Ignore))
            return;



        Renderer rend = hit.transform.GetComponent<Renderer>();
        MeshCollider meshCollider = hit.collider as MeshCollider;
        if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
            return;

        Texture2D tex = rend.material.mainTexture as Texture2D;
        Vector2 pixelUV = hit.textureCoord;
        pixelUV.x *= tex.width;
        pixelUV.y *= tex.height;
        tex.SetPixel(Mathf.FloorToInt(pixelUV.x), Mathf.FloorToInt(pixelUV.y), Color.black);
        tex.Apply();
    }

}

