using UnityEngine;

public class GameManager : MonoBehaviour
{
	[Header("Components")]
    [SerializeField] private Ball ball;
	[SerializeField] private Box[] boxes;
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

		for (int i = 0; i <= boxes.Length - 1; i++)
		{
			boxes[i].IsColliding(false);

			if (CollisionDetection.IsColliding(ball, boxes[i]))
			{
				CollisionDetection.ResolvePenetration(ball, boxes[i]);
				ball.IsColliding(true);
				boxes[i].IsColliding(true);
			}
		}
    }

    private void MouseInput() 
    {
		mouseMotion = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mouseMotion.z = 0f;
		ball.position = mouseMotion;

		//if (Input.GetMouseButton(0)) // 0 for left click, 1 right, 2 middle
		//{
		//	pressedMouseButton = true;
		//	mouseMotion = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//	mouseMotion.z = 0f;
		//}

		//if (Input.GetMouseButtonUp(0))
		//{
		//	pressedMouseButton = false;
		//	Vector2 mouseImpulseDirection = (ball.transform.position - mouseMotion).normalized;
		//	float mouseImpulseMagnitude = (ball.transform.position - mouseMotion).magnitude * mouseStrength;
		//	ball.AddForce(mouseImpulseDirection * mouseImpulseMagnitude);
		//}
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
