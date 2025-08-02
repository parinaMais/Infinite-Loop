using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private int initialLevel;
	[Header("Components")] 
	[SerializeField] private TextMeshPro loopText;
	[SerializeField] private TextMeshPro clickText;
    private Ball ball;
	[Header("Settings")]
	[SerializeField] private float mouseStrength = 5f;
	public static GameManager instance;
	private Dictionary<int, LevelManager> levels = new Dictionary<int, LevelManager>();
	private static int currentLevel = 1;
	
	// Input //
	private Vector3 mouseMotion = Vector3.zero;
	private bool pressedMouseButton = false;
	private bool gameRunning = false;

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
		
		currentLevel = initialLevel;
	}

	public void SetBall(Ball ball)
	{
		this.ball = ball;
		StartCoroutine(SetLevel());
	}

	public void AddLevel(LevelManager level)
	{
		levels[level.Key] = level;
	}

	void Update()
    {
	    if(!gameRunning) return;
		BallMouseInput();
		ResetInput();
    }

	private IEnumerator SetLevel()
	{
		ball.gameObject.SetActive(false);
		ball.SetPosition(levels[currentLevel].BallPosition);
		ball.gameObject.SetActive(true);
		yield return new WaitForEndOfFrame();
		levels[currentLevel].ShowHide(true);
				
		yield return new WaitForSeconds(1f);
				
		gameRunning = true;
	}

	public void ChangeLevel()
	{
		StartCoroutine(ChangeLevelCoroutine());
	}
	
	IEnumerator ChangeLevelCoroutine()
	{
		loopText.gameObject.SetActive(true);
		gameRunning = false;

		yield return new WaitForSeconds(1f);
		
		clickText.gameObject.SetActive(true);

		while (!gameRunning)
		{
			if (Input.anyKeyDown)
			{
				loopText.gameObject.SetActive(false);
				clickText.gameObject.SetActive(false);
				levels[currentLevel].ShowHide(false);
				currentLevel++;

				if (currentLevel > levels.Count)
				{
					Debug.Log("End of game");
					yield break;
				}
				
				StartCoroutine(SetLevel());
			}
			
			yield return null;
		}
	}

	private void FixedUpdate()
	{
		if(ball == null || levels.Count == 0 || !gameRunning) return;
		
		float deltaTime = Time.deltaTime;
		if (deltaTime > 0.016) deltaTime = 0.016f; // cap at ~60FPS, TODO: talvez mudar pra 0.033s que seria 30FPS, verificar

		//ball.AddForce();
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
			else if (levels[currentLevel].Bodies[i] is Triangle triangle)
			{
				for (int j = 0; j < 5; j++)
				{
					if (CollisionDetection.IsCollidingBallTriangle(ball, triangle))
					{
						CollisionDetection.ResolveCollisionTriangle(ball, triangle);
						ball.IsColliding(true);
						triangle.IsColliding(true);
					}
				}
			}
		}

		ball.Integrate(deltaTime);
	}

	private void BallMouseInput() 
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
			OnShoot?.Invoke();
		}
	}

	private void ResetInput()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			StartCoroutine(SetLevel());
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
