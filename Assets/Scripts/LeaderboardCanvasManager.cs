using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;

public class LeaderboardCanvasManager : MonoBehaviour
{

    public GameObject one;
    public TMP_Text name1;
    public TMP_Text score1;
    public GameObject two;
    public GameObject three;
    public GameObject four;
    public GameObject five;
    public DatabaseManager databaseManager;

    // Start is called before the first frame update
    void Start()
    {
        string name = "Teacher";
        int newScore = PlayerPrefs.GetInt("score");
        databaseManager.callMeWhenTheGameReallyEnds(name, newScore, 5, one, two, three, four, five);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
