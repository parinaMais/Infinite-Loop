using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLine : MonoBehaviour
{
	private LineRenderer lineRenderer;

	void Start()
    {
		lineRenderer = GetComponent<LineRenderer>();
	}

	public void DrawLine(Vector3 from, Vector3 to) 
	{
		lineRenderer.positionCount = 2;
		lineRenderer.SetPosition(0, from);
		lineRenderer.SetPosition(1, to);
	}

	public void ClearLine() 
	{
		lineRenderer.positionCount = 0;
	}
}
