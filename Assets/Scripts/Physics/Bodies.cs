using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bodies : MonoBehaviour
{
	public abstract float GetFriction();

	public abstract void IsColliding(bool state);
}
