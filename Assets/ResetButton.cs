﻿using UnityEngine;
using System.Collections;

public class ResetButton : Interactable {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void interact(GameObject interactionGO)
    {
        GameController gc = FindObjectOfType<GameController>();
        LevelCtrl lc = gc.getCurrentLevel();
        if(lc != null)
            lc.resetLevel();
    }
}
