using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Ball_TEST : Body
{
    [SerializeField] private float mass = 1f;

    private MeshRenderer renderer;

	private void Awake()
	{
        radius = transform.localScale.x / 2f;

        renderer = GetComponentInChildren<MeshRenderer>();

		restitution = 1f;
	}

	private void Update()
	{
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //transform.position = new Vector3(mousePos.x, mousePos.y, 0f);

		if (isColliding)
        {
            renderer.material.SetColor("_BaseColor", Color.red);
        }
        else
        {
			renderer.material.SetColor("_BaseColor", Color.white);
		}
	}
}
