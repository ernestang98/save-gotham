using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public GameObject GameoverCanvas;
    public float timeRemaining = 5;
    public bool timerIsRunning = false;
    public Text timeText;
    public Text pointText;
    public AudioSource gameOverAudio;
   
    public TMP_Text ScoreText;

    private void Start()
    {
        PlayerPrefs.SetInt("score", 0);
        // Starts the timer automatically
        timerIsRunning = true;
    }
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                PlayerPrefs.SetString("marks", pointText.text.Substring(8));
                ScoreText.SetText(PlayerPrefs.GetString("marks").ToString());
                GameoverCanvas.SetActive(true);
                gameOverAudio.Play();
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
       
            }
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = "Time: " + string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}