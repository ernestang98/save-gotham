using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PointsController : MonoBehaviour
{
    public Text pointsText;
    public Text pointsReceived;
    public GameObject pointsReceivedCanvas;
    public PollutionManager pollutionManager;
    public DatabaseManager databaseManager;

    public float timeRemaining = 1;
    public bool timerIsRunning = false;

    public float minutes;
    public float seconds;

    public GameObject winCanvas;
    public AudioSource gameCompletedAudio;
    public Text timeText;

    public TMP_Text ScoreText;
    public TMP_Text MinuteText;
    public TMP_Text SecondText;

    public int points;
    private int maxPoints = 200;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.SetString("time", "");

        timerIsRunning = true;
        points = 0;
        updatePointText();
        pollutionManager = GameObject.Find("PollutionManager").GetComponent<PollutionManager>();
    }

   
    // Update is called once per frame
    void FixedUpdate()
    {
        pollutionManager.pollutionIndex = ((float)maxPoints - (float)points) / (float)maxPoints * 3.0f;
    }

    public void addPoints(int pointsToAdd)
    {
        StartCoroutine(showPointsReceived(pointsToAdd));
        points += pointsToAdd;
        updatePointText();

        if (points > 200)
        {
             
            PlayerPrefs.SetInt("score", points);
            PlayerPrefs.SetString("time", timeText.text.Substring(6));
            //send score to database
            //name just put a default one
            ScoreText.SetText(PlayerPrefs.GetInt("score").ToString());
            MinuteText.SetText(PlayerPrefs.GetString("time").ToString());

            
            //databaseManager.UpdateUserScore("Jeremiah", points);

            winCanvas.SetActive(true);
            gameCompletedAudio.Play();

            
        }
    }

  
        

    IEnumerator showPointsReceived(int pointsToAdd)
    {
        if (pointsToAdd > 0)
        {
            pointsReceived.color = new Color(0,0.75f,0,1);
            pointsReceived.text = "+" + pointsToAdd;
        }
        else
        {
            pointsReceived.color = Color.red;
            pointsReceived.text = "" + pointsToAdd;
        }
        for (int i=0; i<5; i++)
        {
            pointsReceivedCanvas.SetActive(true);
            yield return new WaitForSeconds(0.4f);
            pointsReceivedCanvas.SetActive(false);
            yield return new WaitForSeconds(0.4f);
        }
        pointsReceived.color = Color.white;
        pointsReceived.text = "";
    }

    private void updatePointText()
    {
        if (points < 0)
            points = 0;
        pointsText.text = "Points: " + points + " / " + maxPoints;
    }
}
