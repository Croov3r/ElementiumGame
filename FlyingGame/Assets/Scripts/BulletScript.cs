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

	[Header("Innert")]
	[Space]
	Vector3 gravAcceleration = new Vector3(0, -9.8f, 0);
	Vector3 velocity = new Vector3(1, 1, 0);
	public Vector3 position;
	public Vector3 realVelocity = new Vector3(1, 1, 0);

	bool isBouncing;
	Rigidbody2D rigb;

	public void Set(Vector3 pos,  float ang, float s)
	{
		angle = ang;
		speed = s;
		velocity = new Vector3(Mathf.Cos(ang / 180 * Mathf.PI), Mathf.Sin(ang / 180 * Mathf.PI), 0f) * speed;
		position = pos;
		time = 0f;
	}

	public Vector3 GetPoint(float t)
	{
		return 0.5f * gravAcceleration * t * t + velocity * t + position;
	}

	void Destroy()
	{
		Destroy(gameObject);
	}

	void Start()
	{
		transform.position = position;
		Set(position, angle, speed);
		rigb = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		transform.position = GetPoint(time);
		time += Time.deltaTime * timeSpeed;
		realVelocity = (GetPoint(time + 0.001f) - GetPoint(time)) * 1000f;
		transform.Rotate(0f,0f,180f*Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		//Destroy();
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (true)
		{
			Vector3 norm = new Vector3(collision.contacts[0].normal.x, collision.contacts[0].normal.y,0f);
			position = transform.position;
			float strenght = realVelocity.magnitude;
			Vector3 normVel = realVelocity;
			normVel.Normalize();
			float angle = Mathf.Acos(Vector3.Dot(normVel, norm));
		}
	}
	void StopBounce()
	{
		isBouncing = false;
	}
}
