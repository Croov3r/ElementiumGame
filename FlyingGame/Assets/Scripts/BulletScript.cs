using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
	[Header("Custom")]
	public float angle = 0f;
	public float speed = 1f;
	[HideInInspector]
	public float time = 0f;
	public float timeSpeed = 1f;
	float lyingTime = 0f;
	public float maxLyingTime;

	[Header("Innert")]
	[Space]
	Vector3 gravAcceleration = new Vector3(0, -9.8f, 0);

	public Vector3 velocity = new Vector3(1, 1, 0);
	public Vector3 position;
	public Vector3 realVelocity = new Vector3(1, 1, 0);

	public float floorHeight = -4.585f;

	[Range(0f, 1f)]
	public float bouncyness = 0.8f;
	float rotationSpeed = 1f;

	Rigidbody2D rigb;
	[HideInInspector]
	public GameObject stickyPlayer = null;

	public bool inHoop = false;

	public void Set(Vector3 pos,  float ang, float s)
	{
		angle = ang;
		speed = s;
		velocity = new Vector3(Mathf.Cos(ang / 180 * Mathf.PI), Mathf.Sin(ang / 180 * Mathf.PI), 0f) * speed;
		position = pos;
		time = 0f;
		gravAcceleration = new Vector3(0, -9.8f, 0);
		stickyPlayer = null;
		rotationSpeed = 1f;
	}

	public Vector3 GetPoint(float t)
	{
		return 0.5f * gravAcceleration * t * t + velocity * t + position;
	}

	public void StickTo(GameObject go)
	{
		stickyPlayer = go;
		velocity = Vector3.zero;
		gravAcceleration = Vector3.zero;
		rotationSpeed = 0f;
		rigb.angularVelocity = 0f;
		time = 0f;
	}

	public void Drop()
	{
		stickyPlayer = null;
		gravAcceleration = new Vector3(0, -9.8f, 0);
		velocity = Vector3.zero;
		rotationSpeed = 0f;
		position = transform.position;
		rigb.angularVelocity = 0f;
		time = 0f;
	}

	void Destroy()
	{
		Destroy(gameObject);
	}

	void Start()
	{
		lyingTime = 0f;
		transform.position = position;
		rigb = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		transform.Rotate(0f, 0f, 180f * Time.deltaTime * rotationSpeed);
		if (gravAcceleration == Vector3.zero)
		{
			lyingTime += Time.deltaTime;
		}
		if (lyingTime > maxLyingTime)
		{
			//Destroy();
		}
		if (stickyPlayer != null)
		{
			transform.position = stickyPlayer.transform.position;
		}

	}

	void FixedUpdate()
	{
		time += Time.fixedDeltaTime * timeSpeed;
		transform.position = GetPoint(time);
		realVelocity = (GetPoint(time + 0.001f) - GetPoint(time)) * 1000f;
		rigb.velocity = Vector2.zero;
		transform.position = new Vector3(transform.position.x, Mathf.Max(transform.position.y, floorHeight), 0f);
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (inHoop)
		{
			inHoop = false;
			return;
		}

		time = 0f;

		Vector3 norm = new Vector3(collision.contacts[0].normal.x, collision.contacts[0].normal.y, 0f);
		float k = -norm.x / norm.y;
		Vector3 perp = new Vector3(Mathf.Sqrt(1 / (k * k + 1)), Mathf.Sqrt(1 - 1 / (k * k + 1)), 0f);
		position = transform.position;
		float strenght = realVelocity.magnitude;
		Vector3 normVel = realVelocity;
		normVel.Normalize();

		float angle = Mathf.Acos(Vector3.Dot(normVel, perp));
		if (MyMathf.Arg(normVel) > MyMathf.Arg(perp))
		{
			velocity = MyMathf.RotateVector(realVelocity, 2 * angle) * bouncyness;
		}
		else
		{
			velocity = MyMathf.RotateVector(realVelocity, -2 * angle) * bouncyness;
		}

		if (realVelocity.magnitude < 0.5f)
		{
			velocity = Vector3.zero;
			gravAcceleration = Vector3.zero;
			rotationSpeed = 0f;
			rigb.angularVelocity = 0f;
		}

		rotationSpeed *= bouncyness;
		rigb.angularVelocity *= bouncyness;
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		if (inHoop)
		{
			inHoop = false;
		}
	}
}
