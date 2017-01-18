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


    // Update is called once per frame
    void Update () {
	
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
