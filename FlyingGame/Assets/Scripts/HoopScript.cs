using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopScript : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D other)
	{
		float basketPosition = transform.position.x - 1.69f;
		GameObject go = other.gameObject;
		BulletScript script = go.GetComponent<BulletScript>();
		if (script == null)
        {
			return;
        }

		if ((-script.realVelocity.y * 5f > script.realVelocity.x) && (Mathf.Abs(basketPosition - go.transform.position.x) < 0.5f))
        {
			Debug.Log("hit");
		}
        else 
		{
			Debug.Log("miss");
		}
	}
}
