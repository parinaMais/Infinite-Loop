using UnityEngine;

public class GameManager : MonoBehaviour
{
	[Header("Components")]
    [SerializeField] private Ball ball;
	[SerializeField] private Bodies[] bodies;
	[Header("Settings")]
	[SerializeField] private float mouseStrength = 5f;

	// Input //
	private Vector3 mouseMotion = Vector3.zero;
	private bool pressedMouseButton = false;

	void Update()
    {
		MouseInput();

        float deltaTime = Time.deltaTime;
        if (deltaTime > 0.016) deltaTime = 0.016f; // cap at ~60FPS, TODO: talvez mudar pra 0.033s que seria 30FPS, verificar
		
		//ball.AddForce();

		ball.Integrate(deltaTime);

		ball.IsColliding(false);

		for (int i = 0; i <= bodies.Length - 1; i++)
		{
			bodies[i].IsColliding(false);

			if (bodies[i] is Circle circle)
			{
				if (CollisionDetection.IsCollidingBallCircle(ball, circle))
				{
					CollisionDetection.ResolveCollisionCircle(ball, circle);
					ball.IsColliding(true);
					circle.IsColliding(true);
				}
			}
			else if (bodies[i] is Box box)
			{
				if (CollisionDetection.IsCollidingBallBox(ball, box))
				{
					CollisionDetection.ResolveCollisionBox(ball, box);
					ball.IsColliding(true);
					box.IsColliding(true);
				}
			}
		}
    }

    private void MouseInput() 
    {
		if (Input.GetMouseButton(0)) // 0 for left click, 1 right, 2 middle
		{
			pressedMouseButton = true;
			mouseMotion = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mouseMotion.z = 0f;
		}

		if (Input.GetMouseButtonUp(0))
		{
			pressedMouseButton = false;
			Vector2 mouseImpulseDirection = (ball.transform.position - mouseMotion).normalized;
			float mouseImpulseMagnitude = (ball.transform.position - mouseMotion).magnitude * mouseStrength;
			ball.AddForce(mouseImpulseDirection * mouseImpulseMagnitude);
		}
	}

	private void OnDrawGizmos()
	{
		if (pressedMouseButton)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine(ball.transform.position, mouseMotion);
		}
	}
}
