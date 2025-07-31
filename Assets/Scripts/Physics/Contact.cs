using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contact : MonoBehaviour
{
    static Vector2 start;
    static Vector2 end;

    static Vector2 normal;
    static float depth;

    public static void ResolvePenetration(Body a, Body b) 
    {
        float da = depth / (a.GetInvMass() + b.GetInvMass()) * a.GetInvMass();

        a.position -= normal * da;
	}

    public static void ResolveCollision() 
    {

    }
}
