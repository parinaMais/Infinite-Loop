using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[Header("Components")]
    private Ball ball;
	[Header("Settings")]
	[SerializeField] private float mouseStrength = 5f;
	public static GameManager instance;
	private Dictionary<int, LevelManager> levels = new Dictionary<int, LevelManager>();
	private static int currentLevel = 1;
	
	// Input //
	private Vector3 mouseMotion = Vector3.zero;
	private bool pressedMouseButton = false;

	public System.Action OnShoot;

	private void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	public void SetBall(Ball ball)
	{
		this.ball = ball;
		ball.OnLaunch += OnShoot;
	}

	public void AddLevel(LevelManager level)
	{
		levels[level.Key] = level;
	}

	void Update()
    {
		MouseInput();
		ResetInput();
    }

	private void FixedUpdate()
	{
		if(ball == null || levels.Count == 0) return;
		
		float deltaTime = Time.deltaTime;
		if (deltaTime > 0.016) deltaTime = 0.016f; // cap at ~60FPS, TODO: talvez mudar pra 0.033s que seria 30FPS, verificar

		//ball.AddForce();

		ball.Integrate(deltaTime);

		ball.IsColliding(false);

		for (int i = 0; i <= levels[currentLevel].Bodies.Length - 1; i++)
		{
			levels[currentLevel].Bodies[i].IsColliding(false);

			if (levels[currentLevel].Bodies[i] is Circle circle)
			{
				if (CollisionDetection.IsCollidingBallCircle(ball, circle))
				{
					CollisionDetection.ResolveCollisionCircle(ball, circle);
					ball.IsColliding(true);
					circle.IsColliding(true);
				}
			}
			else if (levels[currentLevel].Bodies[i] is Box box)
			{
				for (int j = 0; j < 5; j++)
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
	}

	private void MouseInput() 
    {
		if (Input.GetMouseButton(0)) // 0 for left click, 1 right, 2 middle
		{
			pressedMouseButton = true;
			mouseMotion = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mouseMotion.z = 0f;
		}

		if (Input.GetMouseButtonUp(0) && ball != null)
		{
			pressedMouseButton = false;
			Vector2 mouseImpulseDirection = (ball.transform.position - mouseMotion).normalized;
			float mouseImpulseMagnitude = (ball.transform.position - mouseMotion).magnitude * mouseStrength;
			ball.AddForce(mouseImpulseDirection * mouseImpulseMagnitude);
		}
	}

	private void ResetInput()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
