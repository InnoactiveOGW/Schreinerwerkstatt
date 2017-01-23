using UnityEngine;
using System.Collections;
using System;

public class WoodMaterial : MonoBehaviour
{

    public bool painted;
    // Use this for initialization
    void Start()
    {
        Texture2D texture = gameObject.GetComponent<Renderer>().material.GetTexture("_MainTex") as Texture2D;

        if (texture == null)
        {
            try
            {

            
            texture = new Texture2D(512, 1024, TextureFormat.RGB24, false);
            Color[] pix = texture.GetPixels();
            for (int i = 0; i < 128; i++)
            {
                for (var j = 0; j < 128; j++)
                    texture.SetPixel(i, j, Color.magenta);
            }
            texture.Apply();
            }
            catch(UnityException ex)
            {
                Debug.Log("Unable to read from texture: " + ex.Message);
            }
        }
        else {
            try { 
                texture = Instantiate(texture) as Texture2D;
            }
            catch (UnityException ex)
            {
                Debug.Log("Unable to read from texture: " + ex.Message);
            }
        }
        GetComponent<Renderer>().material.SetTexture("_MainTex", texture);
        // EDITED:
        // war nur ein Versuch, funktioniert nicht so einfach
        //Texture2D texture = new Texture2D(1024, 1024, TextureFormat.RGB24,false);
        //texture.LoadImage(imageAsset.);
        //GetComponent<Renderer>().material.mainTexture = texture;

    }
}
