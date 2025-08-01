using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float mass = 1f;
    [SerializeField] private float radius = 1f;
    [SerializeField] private float maxSpeed = 50f;
    private Vector2 acceleration, sumForces;
    private float invMass;
    private bool isColliding = false;

    public System.Action OnLaunch;
    
    // TODO: nao eh boa pratica, verificar se depois da pra ser private de novo //
    public Vector2 position, velocity;
    
	private void Awake()
	{
        position = transform.position.ToVector2();

        radius /= 2f;

        if (mass != 0.0f) invMass = (1 / mass);
        else invMass = 0.0f;
    }

    private void OnEnable()
    {
        isColliding = false;
        velocity = Vector2.zero;
    }

    private void Start()
    {
        GameManager.instance.SetBall(this);
    }

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
        this.position = transform.position.ToVector2();
    }

    public void AddForce(Vector2 force) 
    {
        sumForces += force;
        OnLaunch?.Invoke();
    }

    private void ClearForces() 
    {
        sumForces = Vector2.zero;
    }

	public void Integrate(float deltaTime) 
    {
		acceleration = sumForces * invMass;

        velocity += sumForces * deltaTime;

        velocity = velocity.normalized * Mathf.Min(velocity.magnitude, maxSpeed);

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
