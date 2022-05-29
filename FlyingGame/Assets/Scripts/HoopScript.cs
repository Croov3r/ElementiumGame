using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopScript : MonoBehaviour
{
	GameHandlerScript gh;
	public Vector3 basketPos = new Vector3(-1.69f, 4.44f, 0f);

	void Start()
	{
		gh = FindObjectOfType<GameHandlerScript>();
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		Vector3 basketPosition = transform.position + basketPos;

		GameObject go = other.gameObject;

		BulletScript script = go.GetComponent<BulletScript>();

		if (script == null)
		{
			return;
		}

		if ((-script.realVelocity.y * 5f > script.realVelocity.x) && (Mathf.Abs(basketPosition.x - go.transform.position.x) < 0.5f))
		{
			//hit
			script.position = basketPosition;
			script.velocity = Vector3.zero;
			script.time = 0f;
			script.inHoop = true;

			gh.IncreasePoints(1);
		}
		else
		{
			//miss
		}
	}
}
