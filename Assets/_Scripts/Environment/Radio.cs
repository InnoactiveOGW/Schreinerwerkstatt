using UnityEngine;
using System.Collections;

public class Radio : Interactable
{

    AudioSource audioClip;

    // Use this for initialization
    void Start()
    {
        audioClip = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public override void interact(GameObject interactionGO)
    {
        if (audioClip.isPlaying)
        {
            audioClip.Stop();
        }
        else
        {
            audioClip.Play();
        }

    }
}
