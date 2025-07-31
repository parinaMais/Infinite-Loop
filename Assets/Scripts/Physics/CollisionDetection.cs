using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public static bool IsColliding(Body a, Body b) 
    {
        Vector2 ab = b.transform.position - a.transform.position;
        float radiusSum = a.GetRadius() + b.GetRadius();

        bool isColliding = ab.magnitude <= (radiusSum);

        if (!isColliding) return false;

        // TODO: guardar informaçoes do contato

        return true;
    }
}
