using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int key;
    [SerializeField] private Vector2 ballPosition;
    [SerializeField] private Bodies[] bodies;

    public int Key => key;
    public Vector2 BallPosition => ballPosition;
    public Bodies[] Bodies => bodies;
    
    private void Start()
    {
        GameManager.instance.AddLevel(this);
    }
}
