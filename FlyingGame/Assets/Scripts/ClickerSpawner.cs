using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickerSpawner : MonoBehaviour
{
    public GameObject clicker;
    public Transform canvasTransform;
    public float rate;

    List<string> letters = new List<string> {"A","S","D","F","H","J","K","L"};
    List<int> letterNumbers = new List<int> {1, 19, 4, 6, 8, 10, 11, 12 };

    float timer = 0;

    void Update()
    {
        if (timer > rate)
        {
            List<int> usedLetters = new List<int>();
            for (int i = 0; i < 3; i++)
            {
                
                GameObject clone = GameObject.Instantiate(clicker, new Vector3(Random.Range(100, 1820), Random.Range(100, 980)), Quaternion.identity, canvasTransform);
                ClickerScript script = clone.GetComponent<ClickerScript>();
                int random = Random.Range(0,7);
                while (usedLetters.Contains(random))
                {
                    random = Random.Range(0, 7);
                    
                }
                usedLetters.Add(random);
                script.keyCode = (KeyCode)(letterNumbers[random] + 96);
                script.text.text = (random+1).ToString();
            }
            timer = 0;
        }
        timer += Time.deltaTime;
    }
}
