using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    LevelControllerInterface currentLevel;

    // Use this for initialization
    void Start () {
	    // TODO
	}
	
	// Update is called once per frame
	void Update () {
	    // TODO
	}

    public void setCurrentLevel(LevelControllerInterface newLevel)
    {
        currentLevel = newLevel;
    }

    public LevelControllerInterface getCurrentLevel()
    {
        return currentLevel;
    }

    public void finishLevel(GameObject finishedObject)
    {
        float score = currentLevel.evaluateConstruction(finishedObject);

        Debug.Log(score);

        // evaluateConstruction
    }
}
