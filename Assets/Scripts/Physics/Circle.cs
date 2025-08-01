using UnityEngine;

public class Circle : Bodies
{
	private float radius, friction;

	private bool isColliding = false;

	private void Awake()
	{
		friction = 1f;
		radius = transform.localScale.x / 2f;
	}

	public override void IsColliding(bool state)
	{
		isColliding = state;
	}

	public override float GetFriction() 
	{
		return friction;
	}

	public float GetRadius()
	{
		return radius;
	}
}
