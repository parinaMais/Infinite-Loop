using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int initialScore;
    [SerializeField] private int penaltyPerSecond;
    [SerializeField] private int penaltyPerShoot;
    [SerializeField] private TextMeshPro scoreText;
    private float timeInSeconds;
    private int shoots;

    private void Start()
    {
        GameManager.instance.OnShoot += () => { shoots++; };
    }

    private void UpdateScore()
    {
        scoreText.text = "score: " + (initialScore - (penaltyPerSecond * Mathf.RoundToInt(timeInSeconds)) - (penaltyPerShoot * shoots)).ToString(); 
    }

    private void Update()
    {
        timeInSeconds += Time.deltaTime;
        UpdateScore();
    }
}
