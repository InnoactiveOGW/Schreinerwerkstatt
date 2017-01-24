using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor;

public class ResetAll : Interactable
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
       SceneManager.LoadScene("MainScene");
      
    }
}
