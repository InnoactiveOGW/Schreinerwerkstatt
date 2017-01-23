using UnityEngine;
using System.Collections;
using System;

public class GameController : MonoBehaviour {

    LevelControllerInterface currentLevel;

    public int progress = 0;
    public int oldProgress = 0;
    public GameObject[] Hints;
    // Use this for initialization
    void Start () {
	    // TODO
	}
	
	// Update is called once per frame
	void Update () {
        // TODO
        CheckandShowHelper();
	}




    private void CheckandShowHelper()
    {
        if (currentLevel == null)
        {
            ObserveProgessLevel0();
        }else
        {
            switch (currentLevel.LevelNumber)
            {
                case 0:
                    break;
                case 1:
                    ObserveProgessLevel1();
                    break;
                default:
                    break;
            }
        }
    }

    private void ObserveProgessLevel1()
    { 
      switch (progress)
        {
            case 0:
                progress = 1;
                break;
            case 1:
                if (CheckIfTagIsPickedUp("Measure"))
                    progress = 2;
                break;
            case 2:
                if (CheckIfTagIsPickedUp("Pencil"))
                    progress = 3;
                break;
            case 3:
                if (CheckIfTagIsPickedUp("Saw"))
                    progress = 4;
                break;
            case 4:
                if (CheckIfTagIsPickedUp("Glue"))
                    progress = 5;
                break;
        }
        
        if(oldProgress != progress)
        {
            showProgress(progress+1);
        }

    }

    private bool CheckIfTagIsPickedUp(string tag)
    {
        GameObject[] foundGameobjects = GameObject.FindGameObjectsWithTag(tag);
        bool any = false;
        foreach (GameObject gamObj in foundGameobjects)
        {

            Pickup pick = gamObj.GetComponent<Pickup>();
            if (pick != null && pick.isPickedup)
                any = true;
        }
        return any;
    }

    private void ObserveProgessLevel0()
    {
        GameObject[] bluePrints = GameObject.FindGameObjectsWithTag("Blueprint");
        bool any = false;
        foreach( GameObject bluePrint in bluePrints)
        {

            Pickup pick = bluePrint.GetComponent<Pickup>();
            if (pick != null && pick.isPickedup)
                any = true;
        }
        if (any) {   showProgress(1);  }
        else {        showProgress(0); }
    }

    private void showProgress(int progNumber)
    {
        foreach (GameObject hint in Hints)
        {
            if (hint.activeSelf)
                hint.SetActive(false);
        }
        if (Hints.Length > progNumber)
        {
            Hints[progNumber].SetActive(true);
        }
           
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
