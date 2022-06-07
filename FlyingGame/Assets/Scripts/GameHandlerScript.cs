using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandlerScript : MonoBehaviour
{
	public Text pointsText;
	public int points = 0;
	PlayerHandler ph;
	List<Vector3> originalPlayerPositions = new List<Vector3> { };
	Vector3 originalBallPosition;
	FadeScript fade;
	public PlayerController mainPlayer;


	public void IncreasePoints(int x)
	{
		points += x;
		pointsText.text = "Points: " + points.ToString();
	}

	public void SetPoints(int x)
	{
		points = x;
		pointsText.text = "Points: " + points.ToString();
	}

	public void Reset()
	{
		for (int i = 0; i < ph.allplayers.Count; i++)
		{
			ph.allplayers[i].transform.position = originalPlayerPositions[i];
		}
		ph.ball.GetComponent<BulletScript>().Set(originalBallPosition,0,0);
		ph.player = mainPlayer;
	}

	void Start()
	{
		SetPoints(0);
		ph = FindObjectOfType<PlayerHandler>();
		foreach (GameObject player in ph.allplayers)
		{
			originalPlayerPositions.Add(player.transform.position);
		}
		originalBallPosition = ph.ball.transform.position;
		fade = FindObjectOfType<FadeScript>();
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		fade.Reset();
	}
}
