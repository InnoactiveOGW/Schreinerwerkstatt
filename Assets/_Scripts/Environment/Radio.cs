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
