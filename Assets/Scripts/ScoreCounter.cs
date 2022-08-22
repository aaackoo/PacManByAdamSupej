using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter instance;
    public Text scoreText;
    private int score = 0;

    public void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        scoreText.text = "YOUR SCORE\n" + score.ToString();
    }

    public void AddPoints()
    {
        score += 1;
        scoreText.text = "YOUR SCORE\n" + score.ToString();
    }
}
