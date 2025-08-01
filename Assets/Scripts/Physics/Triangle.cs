using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : Box
{
	private void Awake()
	{
		width = this.transform.localScale.x;
		height = this.transform.localScale.y;
	}
	
	private void Start()
	{
		localVertices[0] = new Vector2(-width, -height);
		localVertices[1] = new Vector2(width, -height);
		localVertices[2] = new Vector2(0f, height);

		worldVertices[0] = new Vector2(-width, -height);
		worldVertices[1] = new Vector2(width, -height);
		worldVertices[2] = new Vector2(0f, height);

		for (int i = 0; i < worldVertices.Length; i++)
		{
			worldVertices[i] += transform.position.ToVector2();
		}

		for (int i = 0; i < worldVertices.Length; i++)
		{
			Debug.Log(worldVertices[i]);
		}
	}

	public override void IsColliding(bool state)
	{
		isColliding = state;
	}
}
