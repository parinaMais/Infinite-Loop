using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallVisuals : MonoBehaviour
{
	[SerializeField] Ball ball;
	private float radius;

	MeshRenderer meshRenderer;
	Material meshMaterial;

	private void Start()
	{
		radius = (ball.GetRadius() * 2f);

		meshRenderer = GetComponent<MeshRenderer>();
		meshMaterial = meshRenderer.material;
		meshMaterial.SetFloat("_Radius", radius);
	}

	private void Update()
	{
		if (ball.GetIsColliding())
		{
			meshRenderer.material.SetColor("_BaseColor", Color.red);
		}
		else
		{
			meshRenderer.material.SetColor("_BaseColor", Color.white);
		}
	}
}
