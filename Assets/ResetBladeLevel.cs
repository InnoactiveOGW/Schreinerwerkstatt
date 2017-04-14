using UnityEngine;
using System.Collections;

public class ResetBladeLevel : Interactable {

    AudioSource audioClip;

    void Start()
    {
        audioClip = GetComponent<AudioSource>();
    }

    public override void interact(GameObject interactionGO)
    {
        audioClip.Play();
        CuttingGameController gc = FindObjectOfType<CuttingGameController>();
        gc.resetLevel();
    }
}
