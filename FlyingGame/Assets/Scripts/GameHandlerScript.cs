using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandlerScript : MonoBehaviour
{
	public Text pointsText;
	public int points = 0;
	
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

	void Start()
	{
		SetPoints(0);
	}
}
