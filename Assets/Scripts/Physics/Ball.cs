using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Ball : MonoBehaviour
{
    [SerializeField] private float mass = 1f;
    [SerializeField] private float radius = 1f;

    // TODO: nao eh boa pratica, verificar se depois da pra ser private de novo //
    public Vector2 position, velocity;

	private Vector2 acceleration, sumForces;

    private float invMass;

    private bool isColliding = false;

	private void Awake()
	{
        position = transform.position.ToVector2();

        radius /= 2f;

        if (mass != 0.0f) invMass = (1 / mass);
        else invMass = 0.0f;
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

    public void IsColliding(bool state) 
    {
        isColliding = state;
    }

    public bool GetIsColliding() 
    {
        return isColliding;
    }

    public float GetRadius() 
    {
        return radius;
    }
}
