using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyVisuals : MonoBehaviour
{
    [SerializeField] Bodies body;
	[SerializeField] private float duration = 2f; // Time to complete 360°
	private float elapsedTime = 0f;

	MeshRenderer meshRenderer;
	Material meshMaterial;

	private void Start()
	{
		meshRenderer = GetComponent<MeshRenderer>();
		meshMaterial = meshRenderer.material;
	}

	private void Update()
	{
		if (body.GetIsColliding())
		{
			elapsedTime += Time.deltaTime;
			float progress = elapsedTime / duration;
			float newZRotation = Mathf.Lerp(0, 360, progress);

			// Apply rotation (only Z-axis changes)
			transform.eulerAngles = new Vector3(0, 0, newZRotation);

			// Reset after completing 360° (optional for looping)
			if (elapsedTime >= duration)
			{
				elapsedTime = 0f;
			}
		}
	}
}
