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
    }

    private void UpdateScore()
    {
        scoreText.text = "score: " + shoots; 
    }

    private void Update()
    {
        UpdateScore();
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt("score", shoots);
        PlayerPrefs.Save();
    }
}
