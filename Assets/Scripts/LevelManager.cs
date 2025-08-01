using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
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

#if  UNITY_EDITOR
    public void SetBallPosition(Vector2 position)
    {
        ballPosition = position;
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelManager levelManager = (LevelManager)target;

        if (GUILayout.Button("Get Ball Position"))
        {
            Ball ball = FindObjectOfType<Ball>();
            if (ball != null)
            {
                levelManager.SetBallPosition(ball.transform.position);
            }
        }
    }
}

#endif
