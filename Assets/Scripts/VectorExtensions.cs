using UnityEngine;

public static class VectorExtensionMethods
{
	public static Vector3 ToVector3(this Vector2 v)
	{
		return new Vector3(v.x, v.y, 0f);
	}

	public static Vector2 ToVector2(this Vector3 v)
	{
		return new Vector2(v.x, v.y);
	}

	public static Vector2 GetNormal(this Vector2 v) 
	{
		return new Vector2(v.y, -v.x).normalized;
	}

	public static Vector2 Rotate(this Vector2 v, float angle)
	{
		float radians = angle * Mathf.Deg2Rad;
		Vector2 result = new(v.x * Mathf.Cos(radians) - v.y * Mathf.Sin(radians),
							v.x * Mathf.Sin(radians) + v.y * Mathf.Cos(radians));
		return result;
	}
}