using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshPro scoreText;
    [SerializeField] private int skipPenalty = 10;
    private static int shoots;

    private void Start()
    {
        GameManager.instance.OnShoot += () => { shoots++; };
        GameManager.instance.OnSkip += () => { shoots += skipPenalty; };
        GameManager.instance.OnEndGame += SaveScore;
    }

    private void UpdateScore()
    {
        scoreText.text = "score: " + shoots; 
    }

    private void Update()
    {
        UpdateScore();
    }

    private void SaveScore()
    {
        var currentScore = PlayerPrefs.GetInt("score");

        if (currentScore <= 0)
        {
            PlayerPrefs.SetInt("score", shoots);
            PlayerPrefs.Save();
            return;
        }
        
        if (shoots < currentScore)
        {
            PlayerPrefs.SetInt("score", shoots);
            PlayerPrefs.Save();
        }
    }
}
