using UnityEngine;

public class Circle : MonoBehaviour
{
	private float radius;

	private bool isColliding = false;

	private void Awake()
	{
		radius = transform.localScale.x / 2f;
	}

	public void IsColliding(bool state)
	{
		isColliding = state;
	}

	public float GetRadius()
	{
		return radius;
	}
}
