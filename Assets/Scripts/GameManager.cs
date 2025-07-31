using UnityEngine;

public class GameManager : MonoBehaviour
{
	[Header("Components")]
    [SerializeField] private Ball ball;
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

	// TODO: isso tem que ser um raiozinho bonitinho pra build e temos que botar uma artezinha do cursor aparecendo o tempo todo tb
	private void OnDrawGizmos()
	{
		if (pressedMouseButton)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine(ball.transform.position, mouseMotion);
		}
	}
}
