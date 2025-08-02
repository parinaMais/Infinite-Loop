using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : Bodies
{
	private float width, height;

	private bool isColliding = false;

	private Vector2[] localVertices = new Vector2[3];
	private Vector2[] worldVertices = new Vector2[3];

	private Vector2[] triangleNormals = new Vector2[3];

	private void Awake()
	{
		width = this.transform.localScale.x;
		height = this.transform.localScale.y;
	}
	
	private void Start()
	{
		isColliding = false;
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

		for (int i = 0; i < triangleNormals.Length; i++)
		{
			int currVertex = i;
			int nextVertex = (i + 1) % triangleNormals.Length;

			triangleNormals[i] = (worldVertices[currVertex] - worldVertices[nextVertex]).GetNormal();
		}
	}

	public Vector2[] GetWorldVertices()
	{
		return worldVertices;
	}

	public Vector2 GetTriangleNormals(int i)
	{
		return triangleNormals[i];
	}

	public Vector2 EdgeAt(int index)
	{
		int currVertex = index;
		int nextVertex = (index + 1) % worldVertices.Length;
		return worldVertices[nextVertex] - worldVertices[currVertex];
	}

	public override void IsColliding(bool state)
	{
		isColliding = state;
	}

	public override bool GetIsColliding()
	{
		return isColliding;
	}
}
