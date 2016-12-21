using UnityEngine;
using System.Collections;

public class WoodMaterial : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Texture2D texture = new Texture2D(24, 480, TextureFormat.RGB24, false);

        Color[] pix = texture.GetPixels();
        for (int i = 0; i < 128; i++)
        {
            for (var j = 0; j < 128; j++)
                texture.SetPixel(i, j, Color.magenta);
            // pix[i] = Color.white;
        }
        texture.Apply();

        GetComponent<Renderer>().material.SetTexture("_MainTex", texture);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
