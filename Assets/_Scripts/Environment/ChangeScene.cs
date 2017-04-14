using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : Interactable
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
        if(SceneManager.GetActiveScene().name == "MainScene")
             SceneManager.LoadScene("Laser");
        if (SceneManager.GetActiveScene().name == "Laser")
            SceneManager.LoadScene("HammerFall");
        if (SceneManager.GetActiveScene().name == "HammerFall")
            SceneManager.LoadScene("MainScene");
    }
}
