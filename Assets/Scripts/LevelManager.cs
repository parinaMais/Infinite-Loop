using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int key;
    [SerializeField] private Bodies[] bodies;

    public int Key => key;
    public Bodies[] Bodies => bodies;
    
    private void Start()
    {
        GameManager.instance.AddLevel(this);
    }
}
