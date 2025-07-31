using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    protected bool isColliding;

    protected float radius;

    public void IsColliding(bool value) 
    {
        isColliding = value;
    }

    public float GetRadius() 
    {
        return radius;
    }
}
