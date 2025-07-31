using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
	static Vector2 start;
	static Vector2 end;

	static Vector2 normal;
	static float depth;

	public static bool IsColliding(Body a, Body b) 
    {
        Vector2 ab = b.transform.position - a.transform.position;
        float radiusSum = a.GetRadius() + b.GetRadius();

        bool isColliding = ab.magnitude <= (radiusSum);

        if (!isColliding) return false;
		
		normal = ab.normalized;

		start = b.transform.position.ToVector2() - normal * b.GetRadius();
		end = a.transform.position.ToVector2() + normal * a.GetRadius();

		depth = (end - start).magnitude;

        return true;
    }

	public static void ResolvePenetration(Body a, Body b)
	{
		float da = depth / (a.GetInvMass() + b.GetInvMass()) * a.GetInvMass();

		a.position -= normal * da;
	}

	public static void ResolveCollision(Body a, Body b) 
    {

    }
}
