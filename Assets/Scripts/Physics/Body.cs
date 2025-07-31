using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    protected bool isColliding;

	public Vector2 position, velocity;

	protected float radius, invMass, restitution;

    public void IsColliding(bool value) 
    {
        isColliding = value;
    }

    public float GetRadius() 
    {
        return radius;
    }

    public float GetInvMass()
    {
        return invMass;
    }

    public float GetRestitution() 
    {
        return restitution;
    }
}
