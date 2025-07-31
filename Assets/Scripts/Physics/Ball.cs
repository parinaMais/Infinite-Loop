using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Ball : Body
{
    [SerializeField] private float mass = 1f;

	private Vector2 velocity, acceleration, sumForces, position;

    private float invMass;

	private void Awake()
	{
        position.x = transform.position.x;
        position.y = transform.position.y;

        if (mass != 0.0f) invMass = (1 / mass);
        else invMass = 0.0f;

        radius = transform.localScale.x / 2f;
    }

	private void Update()
	{
        Debug.Log(gameObject.name + "is colliding: " + isColliding);
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
