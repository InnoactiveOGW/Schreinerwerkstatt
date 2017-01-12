using UnityEngine;
using System.Collections;

public class GameConfig : MonoBehaviour
{
    // This will be displayed in the inspector as usual
    public float measureSizeFactor_;

    // public property of the variable
    public float measureSizeFactor
    {
        get
        {
            return Config.sizeFactor;
        }
        set
        {
            Debug.Log("Setter!");
            Config.sizeFactor = value;
        }
    }

    //add here 
}