using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private float width, height;

	private bool isColliding = false;

    private Vector2[] localVertices = new Vector2[4];
    private Vector2[] worldVertices = new Vector2[4];

	// TODO DEPOIS REFATORAR ISSO É MUDANÇA DO VISUAL
	private MeshRenderer renderer;

	private void Awake()
	{
        width = this.transform.localScale.x;
        height = this.transform.localScale.y;
	}

	private void Start()
	{
		localVertices[0] = new Vector2(-width / 2.0f, -height / 2.0f);
		localVertices[1] = new Vector2(width / 2.0f, -height / 2.0f);
		localVertices[2] = new Vector2(width / 2.0f, height / 2.0f);
		localVertices[3] = new Vector2(-width / 2.0f, height / 2.0f);

		worldVertices[0] = new Vector2(-width / 2.0f, -height / 2.0f);
		worldVertices[1] = new Vector2(width / 2.0f, -height / 2.0f);
		worldVertices[2] = new Vector2(width / 2.0f, height / 2.0f);
		worldVertices[3] = new Vector2(-width / 2.0f, height / 2.0f);

		for (int i = 0; i < worldVertices.Length; i++)
		{
			worldVertices[i] += transform.position.ToVector2();
		}

		renderer = GetComponentInChildren<MeshRenderer>();
	}

	private void Update()
	{
		if (isColliding)
		{
			renderer.material.SetColor("_BaseColor", Color.red);
		}
		else
		{
			renderer.material.SetColor("_BaseColor", Color.white);
		}
	}

	public Vector2[] GetWorldVertices() 
	{
		return worldVertices;
	}

	public void IsColliding(bool state)
	{
		isColliding = state;
	}

	public Vector2 GetPosition() 
	{
		return transform.position.ToVector2();
	}

	public Vector2 EdgeAt(int index) 
	{
		int currVertex = index;
		int nextVertex = (index + 1) % worldVertices.Length;
		return worldVertices[nextVertex] - worldVertices[currVertex];
	}
}
