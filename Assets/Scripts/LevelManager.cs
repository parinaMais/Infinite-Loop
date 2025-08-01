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

    public void ShowHide(bool show)
    {
        for (var i = 0; i < bodies.Length; i++)
        {
            bodies[i].gameObject.SetActive(show);
        }
    }

#if  UNITY_EDITOR
    public void SetBallPosition(Vector2 position)
    {
        ballPosition = position;
    }

    public void SetBodies(Bodies[] bodies)
    {
        this.bodies = bodies;
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
        
        if (GUILayout.Button("Get Bodies"))
        {
            levelManager.SetBodies(levelManager.GetComponentsInChildren<Bodies>());
        }
    }
}

#endif
