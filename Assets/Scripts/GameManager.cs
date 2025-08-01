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
	[SerializeField] private GameObject infiniteLoopBG;
	[SerializeField] private MouseLine mouseLine;
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
	private bool gameWon = false;

	private bool clickedToContinue = false;
	public System.Action OnShoot;
	public Camera mainCamera;

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

		Application.targetFrameRate = 60;
		QualitySettings.vSyncCount = 0;
		
		currentLevel = initialLevel;
		mainCamera = Camera.main;
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
		if (pressedMouseButton) mouseLine.DrawLine(ball.transform.position + ((ball.transform.position - mouseMotion).normalized) * 0.7f, mouseMotion);
		else mouseLine.ClearLine();
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
		clickedToContinue = false;
	}

	public void ChangeLevel()
	{
		StartCoroutine(ChangeLevelCoroutine());
	}
	
	IEnumerator ChangeLevelCoroutine()
	{
		loopText.gameObject.SetActive(true);
		infiniteLoopBG.gameObject.SetActive(true);
		gameRunning = false;

		yield return new WaitForSeconds(1f);
		
		clickText.gameObject.SetActive(true);

		while (!gameRunning)
		{
			if (Input.GetMouseButton(0) && !clickedToContinue)
			{
				clickedToContinue = true;
				loopText.gameObject.SetActive(false);
				infiniteLoopBG.gameObject.SetActive(false);
				clickText.gameObject.SetActive(false);
				levels[currentLevel].ShowHide(false);
				currentLevel++;

				if (currentLevel > levels.Count)
				{
					Debug.Log("End of game");
					gameWon = true;
					yield break;
				}
				
				StartCoroutine(SetLevel());
			}
			
			yield return null;
		}
	}

	private Vector3 viewPos;
	private float deltaTime;
	private void FixedUpdate()
	{
		if(ball == null || levels.Count == 0 || gameWon || mainCamera == null) return;
		
		deltaTime = Time.deltaTime;
		//if (deltaTime > 0.016) deltaTime = 0.016f; // cap at ~60FPS

		if (gameRunning)
		{
			viewPos = mainCamera.WorldToViewportPoint(ball.transform.position);
			if (viewPos.x < -0.3f || viewPos.x > 1.3f || viewPos.y < -0.3f || viewPos.y > 1.3f)
			{
				StartCoroutine(SetLevel());
			}
		}
		
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
				for (int j = 0; j < 15; j++)
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
			if(timeScaleChanger != null) StopCoroutine(TimeScaleChanger());
			timeScaleChanger = StartCoroutine(TimeScaleChanger());
		}
	}

	Coroutine timeScaleChanger;
	IEnumerator TimeScaleChanger()
	{
		Time.timeScale = 1;
		while (true)
		{
			Time.timeScale = Mathf.Clamp(Time.timeScale + (0.1f * Time.deltaTime), 1f, 3f);
			yield return null;
			
			if(Time.timeScale >= 3) break;
		}
		
		yield return null;
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
