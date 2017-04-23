using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetAll : Interactable
{   
    public override void interact(GameObject interactionGO)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
