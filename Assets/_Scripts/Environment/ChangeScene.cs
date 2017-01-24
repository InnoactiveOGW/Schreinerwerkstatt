using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor;

public class ChangeScene : Interactable
{
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
        if(SceneManager.GetActiveScene().name == "MainScene")
             SceneManager.LoadScene("Laser");
        if (SceneManager.GetActiveScene().name == "Laser")
            SceneManager.LoadScene("HammerFall");
        if (SceneManager.GetActiveScene().name == "HammerFall")
            SceneManager.LoadScene("MainScene");
    }
}
