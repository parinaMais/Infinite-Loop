﻿using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
	static Vector2 start;
	static Vector2 end;

	static float depth;
	static Vector2 normal;

	static Vector2 triangleNormal;

	public static bool IsCollidingBallCircle(Ball ball, Circle circle)
	{
		// Algorithm adapted from Pikuma's Physics Course //
		Vector2 circleToBall = ball.transform.position - circle.transform.position;
		float radiusSum = ball.GetRadius() + circle.GetRadius();

		bool isColliding = circleToBall.magnitude <= (radiusSum);

		if (!isColliding) return false;

		normal = circleToBall.normalized;
		start = circle.transform.position.ToVector2() - (normal * circle.GetRadius());
		end = ball.transform.position.ToVector2() + (normal * ball.GetRadius());
		depth = (end - start).magnitude;

		return true;
	}

	public static bool IsCollidingBallBox(Ball ball, Box box) 
    {
		// Algorithm adapted from Pikuma's Physics Course //
		Vector2[] boxVertices = box.GetWorldVertices();

		bool isOutside = false;
		Vector2 minCurrVertex = Vector2.zero;
		Vector2 minNextVertex = Vector2.zero;
		float distanceCircleEdge = float.MinValue;

		// Loop all the edges of the box finding the nearest edge to the ball's center
		for (int i = 0; i < boxVertices.Length; i++)
		{
			int currVertex = i;
			int nextVertex = (i + 1) % boxVertices.Length; // wraps up the array
			Vector2 edge = box.EdgeAt(currVertex);
			Vector2 normal = edge.GetNormal();

			// Compare the circle center with the box vertex
			Vector2 vertexToCircleCenter = ball.position - boxVertices[currVertex];
			float projection = Vector2.Dot(vertexToCircleCenter, normal);

			// If we found a dot product projection that is in the positive/outside side of the normal
			if (projection > 0)
			{
				// Circle center is outside the polygon
				distanceCircleEdge = projection;
				minCurrVertex = boxVertices[currVertex];
				minNextVertex = boxVertices[nextVertex];
				isOutside = true;
				break;
			}
			else
			{
				// Circle center is inside the rectangle, find the min edge (the one with the least negative projection)
				if (projection > distanceCircleEdge)
				{
					distanceCircleEdge = projection;
					minCurrVertex = boxVertices[currVertex];
					minNextVertex = boxVertices[nextVertex];
				}
			}
		}

		if (isOutside)
		{
			///////////////////////////////////////
			// Check if we are inside region A:
			///////////////////////////////////////
			Vector2 v1 = ball.position - minCurrVertex; // vector from the nearest vertex to the circle center
			Vector2 v2 = minNextVertex - minCurrVertex; // the nearest edge (from curr vertex to next vertex)
			if (Vector2.Dot(v1, v2) < 0)
			{
				// Distance from vertex to circle center is greater than radius... no collision
				if (v1.magnitude > ball.GetRadius())
				{
					return false;
				}
				else
				{
					// Detected collision in region A:
					depth = ball.GetRadius() - v1.magnitude;
					normal = v1.normalized;
					start = ball.position + (normal * -ball.GetRadius());
					end = start + normal * depth;
				}
			}
			else
			{
				///////////////////////////////////////
				// Check if we are inside region B:
				///////////////////////////////////////
				v1 = ball.position - minNextVertex; // vector from the next nearest vertex to the circle center
				v2 = minCurrVertex - minNextVertex;   // the nearest edge
				if (Vector2.Dot(v1, v2) < 0)
				{
					// Distance from vertex to circle center is greater than radius... no collision
					if (v1.magnitude > ball.GetRadius())
					{
						return false;
					}
					else
					{
						// Detected collision in region B:
						depth = ball.GetRadius() - v1.magnitude;
						normal = v1.normalized;
						start = ball.position + (normal * -ball.GetRadius());
						end = start + normal * depth;
					}
				}
				else
				{
					///////////////////////////////////////
					// We are inside region C:
					///////////////////////////////////////
					if (distanceCircleEdge > ball.GetRadius())
					{
						// No collision... Distance between the closest distance and the circle center is greater than the radius.
						return false;
					}
					else
					{
						// Detected collision in region C:
						depth = ball.GetRadius() - distanceCircleEdge;
						normal = (minNextVertex - minCurrVertex).GetNormal();
						start = ball.position - (normal * ball.GetRadius());
						end = start + normal * depth;
					}
				}
			}
		}
		else
		{
			// The center of circle is inside the polygon... it is definitely colliding!
			depth = ball.GetRadius() - distanceCircleEdge;
			normal = (minNextVertex - minCurrVertex).GetNormal();
			start = ball.position - (normal * ball.GetRadius());
			end = start + (normal * depth);
		}

		return true;
	}

	public static bool IsCollidingBallTriangle(Ball ball, Triangle triangle)
	{
		// Algorithm adapted from Pikuma's Physics Course //
		Vector2[] triangleVertices = triangle.GetWorldVertices();

		bool isOutside = false;
		Vector2 minCurrVertex = Vector2.zero;
		Vector2 minNextVertex = Vector2.zero;
		float distanceCircleEdge = float.MinValue;

		triangleNormal = Vector2.zero;

		// Loop all the edges of the box finding the nearest edge to the ball's center
		for (int i = 0; i < triangleVertices.Length; i++)
		{
			int currVertex = i;
			int nextVertex = (i + 1) % triangleVertices.Length; // wraps up the array


			if (currVertex == 0)
			{
				triangleNormal = triangle.GetTriangleNormals(0);
			}

			if (currVertex == 1)
			{
				triangleNormal = triangle.GetTriangleNormals(1);
			}

			if (currVertex == 2)
			{
				triangleNormal = triangle.GetTriangleNormals(2);
			}

			Vector2 edge = triangle.EdgeAt(currVertex);
			Vector2 normal = edge.GetNormal();

			// Compare the circle center with the box vertex
			Vector2 vertexToCircleCenter = ball.position - triangleVertices[currVertex];
			float projection = Vector2.Dot(vertexToCircleCenter, normal);

			// If we found a dot product projection that is in the positive/outside side of the normal
			if (projection > 0)
			{
				// Circle center is outside the polygon
				distanceCircleEdge = projection;
				minCurrVertex = triangleVertices[currVertex];
				minNextVertex = triangleVertices[nextVertex];
				isOutside = true;
				break;
			}
			else
			{
				// Circle center is inside the rectangle, find the min edge (the one with the least negative projection)
				if (projection > distanceCircleEdge)
				{
					distanceCircleEdge = projection;
					minCurrVertex = triangleVertices[currVertex];
					minNextVertex = triangleVertices[nextVertex];
				}
			}
		}

		if (isOutside)
		{
			///////////////////////////////////////
			// Check if we are inside region A:
			///////////////////////////////////////
			Vector2 v1 = ball.position - minCurrVertex; // vector from the nearest vertex to the circle center
			Vector2 v2 = minNextVertex - minCurrVertex; // the nearest edge (from curr vertex to next vertex)
			if (Vector2.Dot(v1, v2) < 0)
			{
				// Distance from vertex to circle center is greater than radius... no collision
				if (v1.magnitude > ball.GetRadius())
				{
					return false;
				}
				else
				{
					// Detected collision in region A:
					depth = ball.GetRadius() - v1.magnitude;
					normal = v1.normalized;
					start = ball.position + (normal * -ball.GetRadius());
					end = start + normal * depth;
				}
			}
			else
			{
				///////////////////////////////////////
				// Check if we are inside region B:
				///////////////////////////////////////
				v1 = ball.position - minNextVertex; // vector from the next nearest vertex to the circle center
				v2 = minCurrVertex - minNextVertex;   // the nearest edge
				if (Vector2.Dot(v1, v2) < 0)
				{
					// Distance from vertex to circle center is greater than radius... no collision
					if (v1.magnitude > ball.GetRadius())
					{
						return false;
					}
					else
					{
						// Detected collision in region B:
						depth = ball.GetRadius() - v1.magnitude;
						normal = v1.normalized;
						start = ball.position + (normal * -ball.GetRadius());
						end = start + normal * depth;
					}
				}
				else
				{
					///////////////////////////////////////
					// We are inside region C:
					///////////////////////////////////////
					if (distanceCircleEdge > ball.GetRadius())
					{
						// No collision... Distance between the closest distance and the circle center is greater than the radius.
						return false;
					}
					else
					{
						// Detected collision in region C:
						depth = ball.GetRadius() - distanceCircleEdge;
						normal = (minNextVertex - minCurrVertex).GetNormal();
						start = ball.position - (normal * ball.GetRadius());
						end = start + normal * depth;
					}
				}
			}
		}
		else
		{
			// The center of circle is inside the polygon... it is definitely colliding!
			depth = ball.GetRadius() - distanceCircleEdge;
			normal = (minNextVertex - minCurrVertex).GetNormal();
			start = ball.position - (normal * ball.GetRadius());
			end = start + (normal * depth);
		}

		return true;
	}

	public static void ResolvePenetrationReflection(Ball ball)
	{
		Vector2 velocityDirection = (ball.velocity - 2 * (Vector2.Dot(ball.velocity, normal)) * normal).normalized;

		ball.position += velocityDirection * depth;
	}

	public static void ResolvePenetrationNormal(Ball ball)
	{
		ball.position += normal * depth;
	}

	public static void ResolveCollisionBox(Ball ball, Box box) 
    {
		// Reflection formula
		Vector2 velocityDirection = (ball.velocity - 2 * (Vector2.Dot(ball.velocity, normal)) * normal).normalized;
		float velocityMagnitude = ball.velocity.magnitude;

		ball.velocity = velocityDirection * velocityMagnitude;
    }

	public static void ResolveCollisionCircle(Ball ball, Circle circle)
	{
		// Flip
		ball.velocity *= -1f;
	}

	public static void ResolveCollisionTriangle(Ball ball, Triangle triangle)
	{
		//// Velocity normal
		Vector2 velocityDirection = -triangleNormal;
		float velocityMagnitude = ball.velocity.magnitude;

		ball.velocity = velocityDirection * velocityMagnitude;
	}
}
