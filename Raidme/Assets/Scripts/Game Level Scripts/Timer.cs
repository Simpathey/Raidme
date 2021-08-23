using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Timer : MonoBehaviour
{
    public float timeRemaining = 0;
    public bool timerIsRunning = false;
    public TextMeshPro textMeshPro;
    public GameManager gameManager;

    public void AddTime(float time)
    {
        timeRemaining = timeRemaining + time;
    }

    public void StartClock()
    {
        timerIsRunning = true; 
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                //Time ran out do the thing!
                timeRemaining = 0;
                timerIsRunning = false;
                gameManager.StartBattle();
                timeRemaining = 1;
            }
            DisplayTime(timeRemaining);
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        textMeshPro.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
