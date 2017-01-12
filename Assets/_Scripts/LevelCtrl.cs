﻿using UnityEngine;
using System.Collections;

public class LevelCtrl : MonoBehaviour {

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
}
