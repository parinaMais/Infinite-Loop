using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshPro scoreText;
    private static int shoots;

    private void Start()
    {
        GameManager.instance.OnShoot += () => { shoots++; };
    }

    private void UpdateScore()
    {
        scoreText.text = "score: " + shoots; 
    }

    private void Update()
    {
        UpdateScore();
    }
}
