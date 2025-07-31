using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Body
{
    private float width, height;

    private Vector2[] localVertices = new Vector2[4];
    private Vector2[] worldVertices = new Vector2[4];

	private void Awake()
	{
        width = this.transform.localScale.x;
        height = this.transform.localScale.y;

		radius = width;
	}

	private void Start()
	{
		localVertices[0] = new Vector2(-width / 2.0f, -height / 2.0f);
		localVertices[1] = new Vector2(width / 2.0f, -height / 2.0f);
		localVertices[2] = new Vector2(width / 2.0f, height / 2.0f);
		localVertices[3] = new Vector2(-width / 2.0f, height / 2.0f);

		//worldVertices = localVertices; //TODO: verificar se não posso fazer isso, acho que não por array ser ref type, mas confirmar

		worldVertices[0] = new Vector2(-width / 2.0f, -height / 2.0f);
		worldVertices[1] = new Vector2(width / 2.0f, -height / 2.0f);
		worldVertices[2] = new Vector2(width / 2.0f, height / 2.0f);
		worldVertices[3] = new Vector2(-width / 2.0f, height / 2.0f);

		for (int i = 0; i < worldVertices.Length; i++)
		{
			worldVertices[i] += transform.position.ToVector2();
		}

		//for (int i = 0; i < 3; i++)
		//{
		//	Debug.Log(gameObject.name + "Local " + localVertices[i]);
		//	Debug.Log(gameObject.name + "World " + worldVertices[i]);
		//}
	}
}
