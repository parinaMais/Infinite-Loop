using UnityEngine;

public class Circle : Bodies
{
	private float radius;

	private bool isColliding = false;

	private void Awake()
	{
		radius = transform.localScale.x / 2f;
	}

	public override void IsColliding(bool state)
	{
		isColliding = state;
	}

	public float GetRadius()
	{
		return radius;
	}

	public override bool GetIsColliding()
	{
		return isColliding;
	}
}
