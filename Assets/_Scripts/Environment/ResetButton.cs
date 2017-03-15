using UnityEngine;
using System.Collections;

public class ResetButton : Interactable {

    // Use this for initialization
    AudioSource audioClip;
    // Use this for initialization
    void Start()
    {
        audioClip = GetComponent<AudioSource>();
    }

    public override void interact(GameObject interactionGO)
    {
        audioClip.Play();
        GameController gc = FindObjectOfType<GameController>();
        LevelControllerInterface lc = gc.getCurrentLevel();
        if(lc != null)
            lc.resetLevel();
    }
}
