using UnityEngine;
using System.Collections;

public class ResetButton : Interactable {
    
    AudioSource audioClip;

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
