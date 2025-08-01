using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatmapLoopDetection : MonoBehaviour
{
    [SerializeField] private Ball ball;
    public float cellSize = 0.5f;
    public int visitThreshold = 10;
    private Dictionary<Vector2Int, int> heatmap = new Dictionary<Vector2Int, int>();

    public System.Action OnLoopDetected;

    private void Awake()
    {
        ball.OnLaunch += VerifyLoop;
    }

    private void VerifyLoop()
    {
        heatmap.Clear();
        enabled = true;
    }

    private Vector2 pos;
    private Vector2Int cell;
    private Vector2Int lastCell;
    private void Update()
    {
        pos = ball.position;
        cell = new Vector2Int(Mathf.FloorToInt(pos.x / cellSize), Mathf.FloorToInt(pos.y / cellSize));
        if (cell != lastCell)
        {
            if (heatmap.ContainsKey(cell))
            {
                heatmap[cell]++;
            }
            else
                heatmap[cell] = 1;

            if (heatmap[cell] >= visitThreshold)
            {
                OnLoopDetected?.Invoke();
                Debug.Log("Loop detected");
                enabled = false;
            }

            lastCell = cell;
        }
    }
    
    void OnDrawGizmos()
    {
        if (heatmap == null) return;

        foreach (var kvp in heatmap)
        {
            Vector2Int cell = kvp.Key;
            int visits = kvp.Value;

            // 0 → verde; >=visitThreshold → vermelho
            float t = Mathf.Clamp01(visits / (float)visitThreshold);
            Gizmos.color = new Color(t, 1f - t, 0f, 0.85f);  // verde→amarelo→vermelho

            Vector3 center = new Vector3(cell.x * cellSize + cellSize / 2f, cell.y * cellSize + cellSize / 2f, 0f);
            Gizmos.DrawWireCube(center, Vector3.one * cellSize);
        }
    }
}
