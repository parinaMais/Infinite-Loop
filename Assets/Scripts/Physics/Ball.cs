using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Ball : Body
{
    [SerializeField] private float mass = 1f;

	private Vector2 velocity, acceleration, sumForces;

    // TODO: depois tirar
    private MeshRenderer renderer;

	private void Awake()
	{
        position.x = transform.position.x;
        position.y = transform.position.y;

        if (mass != 0.0f) invMass = (1 / mass);
        else invMass = 0.0f;

        radius = transform.localScale.x / 2f;

        renderer = GetComponentInChildren<MeshRenderer>();
    }

    // DEBUG - PRA VERIFICAR A COLISAO // TODO: depois tirar
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

	public void AddForce(Vector2 force) 
    {
        sumForces += force;
    }

    private void ClearForces() 
    {
        sumForces = Vector2.zero;
    }

	public void Integrate(float deltaTime) 
    {
		acceleration = sumForces * invMass;

        velocity += acceleration * deltaTime;

        position += velocity * deltaTime;

        transform.position = position.ToVector3();

        ClearForces();
    }
}
