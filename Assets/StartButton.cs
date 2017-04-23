using UnityEngine;
using System.Collections;

public class StartButton : Interactable {

    AudioSource audioClip;

    void Start()
    {
        audioClip = GetComponent<AudioSource>();
    }

    public override void interact(GameObject interactionGO)
    {
        audioClip.Play();
        EvalController.Instance.startStudy(interactionGO);
    }
}
