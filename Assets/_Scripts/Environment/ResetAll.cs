using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetAll : Interactable
{
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
       SceneManager.LoadScene("MainScene");
    }
}
