using UnityEngine;
using System.Collections;

public class CuttingGameController : GameController {

    public int difficulty = 1;
    public float spawnRate = 0.2f;
    public float maxPieces = 3;
    public float speed = 5;

    public int score = 0;
    public int miss = 0;

    public bool playing = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //if (miss >= 3)
        //    playing = false;
	}

    public void addScore(int newScore) {
        score += newScore;
    }

    public void addMiss(int newMiss)
    {
        miss += newMiss;
    }


}
