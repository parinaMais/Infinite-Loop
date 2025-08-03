using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallVisuals : MonoBehaviour
{
	[SerializeField] Ball ball;
	[SerializeField] Color color;
	[SerializeField] ParticleSystem particle;
	private float radius;

	MeshRenderer meshRenderer;
	Material meshMaterial;
	TrailRenderer trail;

	private void Awake()
	{
		trail = GetComponent<TrailRenderer>();
	}

	private void OnEnable()
	{
		trail.Clear();
	}

	private void Start()
	{
		radius = (ball.GetRadius() * 2f);

		meshRenderer = GetComponent<MeshRenderer>();
		meshMaterial = meshRenderer.material;
		meshMaterial.SetFloat("_Radius", radius);
	}

	private float timer;
	private void Update()
	{
		timer += Time.deltaTime;
		if (ball.GetIsColliding())
		{
			timer = 0;
			meshRenderer.material.SetColor("_BaseColor", color);
			particle.Play();
		}
		else if(timer >= .2f)
		{
			meshRenderer.material.SetColor("_BaseColor", Color.white);
			particle.Stop();
		}
	}
}
