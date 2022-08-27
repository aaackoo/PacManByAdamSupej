using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter instance;
    public GameObject WinningText;
    public GameObject LoosingText;
    public Text scoreText;
    public Text highscoreText;
    public int score;
    public int highscore;
    public int PelletCounter;

    public void Awake()
    {
        instance = this;
    }

    public void YouWon()
    {
        PelletCounter = 300;
        WinningText.SetActive(true);
    }
    
    public void YouLost()
    {
        PelletCounter = 300;
        LoosingText.SetActive(true);
    }

    public void RemoveText()
    {
        WinningText.SetActive(false);
        LoosingText.SetActive(false);
    }
    
    void Start()
    {
        score = 0;
        PelletCounter = 300;
        highscore = PlayerPrefs.GetInt("highscore", 0);
        scoreText.text = "YOUR SCORE\n" + score.ToString();
        highscoreText.text = "Highscore\n" + highscore.ToString();
    }

    public void AddPoints(int value)
    {
        score += value;
        scoreText.text = "YOUR SCORE\n" + score.ToString();
        if (highscore < score)
            PlayerPrefs.SetInt("highscore", score);
        
    }
}