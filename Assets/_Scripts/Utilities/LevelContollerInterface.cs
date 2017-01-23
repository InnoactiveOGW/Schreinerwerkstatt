using UnityEngine;
using System.Collections;

public class LevelControllerInterface : MonoBehaviour {


    public int LevelNumber = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void startLevel()
    {
        GameController gc = FindObjectOfType<GameController>();
        if (gc.getCurrentLevel() != null)
            throw new UnityException("A level is already in progress. Please end the level before trying to start a new one");
        gc.setCurrentLevel(this);
    }

    public virtual void resetLevel()
    {

    }

    public virtual void endLevel()
    {


    }

    public virtual float evaluateConstruction(GameObject go)
    {
        // -1 indicates that the evaluation couldn't be completed
        return -1;
    }

}
