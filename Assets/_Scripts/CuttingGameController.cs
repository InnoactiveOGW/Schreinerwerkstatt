using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CuttingGameController : GameController {

    public int difficulty = 1;
    public float spawnRate = 0.2f;
    public float maxPieces = 3;
    public float speed = 5;

    public int score = 0;
    public int miss = 0;

    public GameObject scoreCounter;
    public GameObject missCounter;
    private TextMesh txtScore;
    private TextMesh txtMiss;

    public bool playing = false;
    

	// Use this for initialization
	void Start () {
        txtScore = scoreCounter.GetComponent<TextMesh>();
        txtMiss = missCounter.GetComponent<TextMesh>();
    }

    public void addScore(int newScore) {
        score += newScore;
        txtScore.text = score.ToString();
    }

    public void addMiss(int newMiss)
    {
        miss += newMiss;
        txtMiss.text = miss.ToString();
    }

    public void resetLevel()
    {
        score = 0;
        miss = 0;
        txtScore.text = score.ToString();
        txtMiss.text = miss.ToString();
        playing = true;
    }
}
