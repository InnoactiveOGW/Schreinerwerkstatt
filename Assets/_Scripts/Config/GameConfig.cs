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
            Config.sizeFactor = value;
        }
    }

	public bool isVR_;

	// public property of the variable
	public bool isVR
	{
		get
		{
			return Config.isVR;
		}
		set
		{
			Config.isVR = value;
		}
	}

    //add here 
}