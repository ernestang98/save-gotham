using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;


public class DatabaseManager : MonoBehaviour
{
    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;    
    public FirebaseUser User;
    public DatabaseReference DBreference;

    public TMP_Text name1;
    public TMP_Text score1;
    public TMP_Text name2;
    public TMP_Text score2;
    public TMP_Text name3;
    public TMP_Text score3;
    public TMP_Text name4;
    public TMP_Text score4;
    public TMP_Text name5;
    public TMP_Text score5;

    private bool connecting;
    private bool connected;

    void Awake()
    {
/*        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                        //If they are avalible Initialize Firebase
                        InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });*/
        CheckConnectionAsync();
        InitializeFirebase();
    }

    public async UniTask<bool> CheckConnectionAsync()
    {

        await UniTask.WaitUntil(() => connecting == false);

        if (connected)
        {
            return true;
        }

        connecting = true;
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync().AsUniTask();
        connected = dependencyStatus == DependencyStatus.Available;
        connecting = false;
        InitializeFirebase();
        return connected;
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void testSubmitScore()
    {
        string name = "Mr Tan";
        StartCoroutine(CreateNewUserCoroutine(name));
/*        StartCoroutine(UpdateUserScoreCoroutine(name, 55));*/
        Debug.Log("send success");
    }

    public void testLeaderboard()
    {
        Debug.Log("display success");
    }

    public int callMeToCreateNewUser(string name)
    {
        CheckConnectionAsync();
        StartCoroutine(CreateNewUserCoroutine(name));
        return 0;
    }

    IEnumerator CreateNewUserCoroutine(string name)
    {
        //Set the currently logged in user xp
        var DBTask = DBreference.Child("users").Child(NameConversion(name)).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            if (DBTask.Result.GetValue(true) == null)
            {
               var DBTask1 = DBreference.Child("users").Child(NameConversion(name)).Child("name").SetValueAsync(name);
                yield return new WaitUntil(predicate: () => DBTask1.IsCompleted);
                if (DBTask1.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task 1 with {DBTask1.Exception}");
                }
                else
                {
                    var DBTask2 = DBreference.Child("users").Child(NameConversion(name)).Child("points").SetValueAsync(0);
                    yield return new WaitUntil(predicate: () => DBTask1.IsCompleted);
                    if (DBTask1.Exception != null)
                    {
                        Debug.LogWarning(message: $"Failed to register task 1 with {DBTask1.Exception}");
                    }
                    else
                    {

                    }
                }
            }
            else
            {
                Debug.LogWarning(message: $"User {name} already exists...");
            }
        }
    }

    public int callMeToUpdateUserScoer(string name, int score)
    {
        CheckConnectionAsync();
        StartCoroutine(UpdateUserScoreCoroutine(name, score));
        return 0;
    }

    IEnumerator UpdateUserScoreCoroutine(string name, int score)
    {
        //Set the currently logged in user xp
        var DBTask = DBreference.Child("users").Child(NameConversion(name)).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            if (DBTask.Result.GetValue(true) != null)
            {
                var DBTask1 = DBreference.Child("users").Child(NameConversion(name)).Child("points").GetValueAsync();

                yield return new WaitUntil(predicate: () => DBTask1.IsCompleted);

                if (DBTask1.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
                }
                else
                {
                    Debug.LogWarning(DBTask1.Result.GetValue(true));
                    if (DBTask1.Result.GetValue(true) == null)
                    {
                        Debug.LogWarning($"User {name} does not have a score yet!");
                        var DBTask2 = DBreference.Child("users").Child(NameConversion(name)).Child("points").SetValueAsync(score);
                        yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);
                        if (DBTask2.Exception != null)
                        {
                            Debug.LogWarning(message: $"Failed to register task2 with {DBTask2.Exception}");
                        }
                        else
                        {
                     
                        }
                    }
                    else
                    {
                        int value = int.Parse(DBTask1.Result.GetValue(true).ToString());
                        if (score > value)
                        {
                            Debug.LogWarning($"User {name} database score is {value} which is lower than the current score of {score}");
                            var DBTask2 = DBreference.Child("users").Child(NameConversion(name)).Child("points").SetValueAsync(score);
                            yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);
                            if (DBTask2.Exception != null)
                            {
                                Debug.LogWarning(message: $"Failed to register task2 with {DBTask2.Exception}");
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            Debug.LogWarning($"User {name} database score is {value} which is higher or equal to the current score of {score}... Do nothing...");
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning(message: $"User {name} does not exist yet...");
            }
        }
    }

    public int callMeWhenTheGameEnds(GameObject go1, GameObject go2, GameObject go3, GameObject go4, GameObject go5)
    {
        // when you call this, user must already exist... if not will throw error
        StartCoroutine(RetrieveTopNHighScoreCoroutine(5, go1, go2, go3, go4, go5));
        return 0;
    }
    
    IEnumerator RetrieveTopNHighScoreCoroutine(int N, GameObject go1, GameObject go2, GameObject go3, GameObject go4, GameObject go5)
    {
        int count = 1;
        var DBTask = FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            if (DBTask.Result.GetValue(true) != null)
            {
                var DBTask1 = DBreference.Child("users").OrderByChild("points").GetValueAsync();

                yield return new WaitUntil(predicate: () => DBTask1.IsCompleted);

                if (DBTask1.Exception == null)
                {
                    DataSnapshot snapshot = DBTask1.Result;
                    foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
                    {
                        if (count <= N)
                        {
                            string name = childSnapshot.Child("name").Value.ToString();
                            int points = int.Parse(childSnapshot.Child("points").Value.ToString());
/*                            var playerdetails = new Dictionary<string, int>();
                            playerdetails.Add(name, points);
                            highscore.Add(playerdetails);*/
                            Debug.Log($"Player {name}'s high score is {points}");

                            switch (count)
                            {
                                case 1:
                                    go1.SetActive(true);
                                    GameObject thename = GetChildWithName(go1, "name");
                                    TMP_Text thenametext = thename.GetComponent<TMP_Text>();
                                    GameObject thescore = GetChildWithName(go1, "score");
                                    TMP_Text thescoretext = thescore.GetComponent<TMP_Text>();
                                    thenametext.text = name;
                                    thescoretext.text = points.ToString();
                                    break;

                                case 2:
                                    go2.SetActive(true);
                                    thename = GetChildWithName(go2, "name");
                                    thenametext = thename.GetComponent<TMP_Text>();
                                    thescore = GetChildWithName(go2, "score");
                                    thescoretext = thescore.GetComponent<TMP_Text>();
                                    thenametext.text = name;
                                    thescoretext.text = points.ToString();
                                    break;

                                case 3:
                                    go3.SetActive(true);
                                    thename = GetChildWithName(go3, "name");
                                    thenametext = thename.GetComponent<TMP_Text>();
                                    thescore = GetChildWithName(go3, "score");
                                    thescoretext = thescore.GetComponent<TMP_Text>();
                                    thenametext.text = name;
                                    thescoretext.text = points.ToString();
                                    break;

                                case 4:
                                    go4.SetActive(true);
                                    thename = GetChildWithName(go4, "name");
                                    thenametext = thename.GetComponent<TMP_Text>();
                                    thescore = GetChildWithName(go4, "score");
                                    thescoretext = thescore.GetComponent<TMP_Text>();
                                    thenametext.text = name;
                                    thescoretext.text = points.ToString();
                                    break;

                                case 5:
                                    go5.SetActive(true);
                                    thename = GetChildWithName(go5, "name");
                                    thenametext = thename.GetComponent<TMP_Text>();
                                    thescore = GetChildWithName(go5, "score");
                                    thescoretext = thescore.GetComponent<TMP_Text>();
                                    thenametext.text = name;
                                    thescoretext.text = points.ToString();
                                    break;

                                default:
                                    break;
                            }

                            count += 1;
                        }
                    }
                }
                else
                {
                    Debug.LogWarning(DBTask1.Exception);
                }

            }
            else
            {
                Debug.LogWarning(message: $"Leaderboard is empty");
            }
        }
    }

    public void callMeWhenTheGameReallyEnds(string name, int score, int N, GameObject go1, GameObject go2, GameObject go3, GameObject go4, GameObject go5)
    {
        StartCoroutine(UpdateUserScoreThenRetrieveTopNHighScoreCoroutine(name, score, N, go1, go2, go3, go4, go5));
    }

    IEnumerator UpdateUserScoreThenRetrieveTopNHighScoreCoroutine(string name, int score, int N, GameObject go1, GameObject go2, GameObject go3, GameObject go4, GameObject go5)
    {
        //Set the currently logged in user xp
        var DBTask = DBreference.Child("users").Child(NameConversion(name)).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            if (DBTask.Result.GetValue(true) != null)
            {
                var DBTask1 = DBreference.Child("users").Child(NameConversion(name)).Child("points").GetValueAsync();

                yield return new WaitUntil(predicate: () => DBTask1.IsCompleted);

                if (DBTask1.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
                }
                else
                {
                    Debug.LogWarning(DBTask1.Result.GetValue(true));
                    if (DBTask1.Result.GetValue(true) == null)
                    {
                        Debug.LogWarning($"User {name} does not have a score yet!");
                        var DBTask2 = DBreference.Child("users").Child(NameConversion(name)).Child("points").SetValueAsync(score);
                        yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);
                        if (DBTask2.Exception != null)
                        {
                            Debug.LogWarning(message: $"Failed to register task2 with {DBTask2.Exception}");
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        int value = int.Parse(DBTask1.Result.GetValue(true).ToString());
                        if (score > value)
                        {
                            Debug.LogWarning($"User {name} database score is {value} which is lower than the current score of {score}");
                            var DBTask2 = DBreference.Child("users").Child(NameConversion(name)).Child("points").SetValueAsync(score);
                            yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);
                            if (DBTask2.Exception != null)
                            {
                                Debug.LogWarning(message: $"Failed to register task2 with {DBTask2.Exception}");
                            }
                            else
                            {
                                int count = 1;
                                DBTask = FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync();

                                yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

                                if (DBTask.Exception != null)
                                {
                                    Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
                                }
                                else
                                {
                                    if (DBTask.Result.GetValue(true) != null)
                                    {
                                        DBTask1 = DBreference.Child("users").OrderByChild("points").GetValueAsync();

                                        yield return new WaitUntil(predicate: () => DBTask1.IsCompleted);

                                        if (DBTask1.Exception == null)
                                        {
                                            DataSnapshot snapshot = DBTask1.Result;
                                            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
                                            {
                                                if (count <= N)
                                                {
                                                    name = childSnapshot.Child("name").Value.ToString();
                                                    int points = int.Parse(childSnapshot.Child("points").Value.ToString());
                                                    /*                            var playerdetails = new Dictionary<string, int>();
                                                                                playerdetails.Add(name, points);
                                                                                highscore.Add(playerdetails);*/
                                                    Debug.Log($"Player {name}'s high score is {points}");

                                                    switch (count)
                                                    {
                                                        case 1:
                                                            go1.SetActive(true);
                                                            GameObject thename = GetChildWithName(go1, "name");
                                                            TMP_Text thenametext = thename.GetComponent<TMP_Text>();
                                                            GameObject thescore = GetChildWithName(go1, "score");
                                                            TMP_Text thescoretext = thescore.GetComponent<TMP_Text>();
                                                            thenametext.text = name;
                                                            thescoretext.text = points.ToString();
                                                            break;

                                                        case 2:
                                                            go2.SetActive(true);
                                                            thename = GetChildWithName(go2, "name");
                                                            thenametext = thename.GetComponent<TMP_Text>();
                                                            thescore = GetChildWithName(go2, "score");
                                                            thescoretext = thescore.GetComponent<TMP_Text>();
                                                            thenametext.text = name;
                                                            thescoretext.text = points.ToString();
                                                            break;

                                                        case 3:
                                                            go3.SetActive(true);
                                                            thename = GetChildWithName(go3, "name");
                                                            thenametext = thename.GetComponent<TMP_Text>();
                                                            thescore = GetChildWithName(go3, "score");
                                                            thescoretext = thescore.GetComponent<TMP_Text>();
                                                            thenametext.text = name;
                                                            thescoretext.text = points.ToString();
                                                            break;

                                                        case 4:
                                                            go4.SetActive(true);
                                                            thename = GetChildWithName(go4, "name");
                                                            thenametext = thename.GetComponent<TMP_Text>();
                                                            thescore = GetChildWithName(go4, "score");
                                                            thescoretext = thescore.GetComponent<TMP_Text>();
                                                            thenametext.text = name;
                                                            thescoretext.text = points.ToString();
                                                            break;

                                                        case 5:
                                                            go5.SetActive(true);
                                                            thename = GetChildWithName(go5, "name");
                                                            thenametext = thename.GetComponent<TMP_Text>();
                                                            thescore = GetChildWithName(go5, "score");
                                                            thescoretext = thescore.GetComponent<TMP_Text>();
                                                            thenametext.text = name;
                                                            thescoretext.text = points.ToString();
                                                            break;

                                                        default:
                                                            break;
                                                    }

                                                    count += 1;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Debug.LogWarning(DBTask1.Exception);
                                        }

                                    }
                                    else
                                    {
                                        Debug.LogWarning(message: $"Leaderboard is empty");
                                    }
                                }
                            }
                        }
                        else
                        {
                            Debug.LogWarning($"User {name} database score is {value} which is higher or equal to the current score of {score}... Do nothing...");
                            int count = 1;
                            DBTask = FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync();

                            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

                            if (DBTask.Exception != null)
                            {
                                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
                            }
                            else
                            {
                                if (DBTask.Result.GetValue(true) != null)
                                {
                                    DBTask1 = DBreference.Child("users").OrderByChild("points").GetValueAsync();

                                    yield return new WaitUntil(predicate: () => DBTask1.IsCompleted);

                                    if (DBTask1.Exception == null)
                                    {
                                        DataSnapshot snapshot = DBTask1.Result;
                                        foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
                                        {
                                            if (count <= N)
                                            {
                                                name = childSnapshot.Child("name").Value.ToString();
                                                int points = int.Parse(childSnapshot.Child("points").Value.ToString());
                                                /*                            var playerdetails = new Dictionary<string, int>();
                                                                            playerdetails.Add(name, points);
                                                                            highscore.Add(playerdetails);*/
                                                Debug.Log($"Player {name}'s high score is {points}");

                                                switch (count)
                                                {
                                                    case 1:
                                                        go1.SetActive(true);
                                                        GameObject thename = GetChildWithName(go1, "name");
                                                        TMP_Text thenametext = thename.GetComponent<TMP_Text>();
                                                        GameObject thescore = GetChildWithName(go1, "score");
                                                        TMP_Text thescoretext = thescore.GetComponent<TMP_Text>();
                                                        thenametext.text = name;
                                                        thescoretext.text = points.ToString();
                                                        break;

                                                    case 2:
                                                        go2.SetActive(true);
                                                        thename = GetChildWithName(go2, "name");
                                                        thenametext = thename.GetComponent<TMP_Text>();
                                                        thescore = GetChildWithName(go2, "score");
                                                        thescoretext = thescore.GetComponent<TMP_Text>();
                                                        thenametext.text = name;
                                                        thescoretext.text = points.ToString();
                                                        break;

                                                    case 3:
                                                        go3.SetActive(true);
                                                        thename = GetChildWithName(go3, "name");
                                                        thenametext = thename.GetComponent<TMP_Text>();
                                                        thescore = GetChildWithName(go3, "score");
                                                        thescoretext = thescore.GetComponent<TMP_Text>();
                                                        thenametext.text = name;
                                                        thescoretext.text = points.ToString();
                                                        break;

                                                    case 4:
                                                        go4.SetActive(true);
                                                        thename = GetChildWithName(go4, "name");
                                                        thenametext = thename.GetComponent<TMP_Text>();
                                                        thescore = GetChildWithName(go4, "score");
                                                        thescoretext = thescore.GetComponent<TMP_Text>();
                                                        thenametext.text = name;
                                                        thescoretext.text = points.ToString();
                                                        break;

                                                    case 5:
                                                        go5.SetActive(true);
                                                        thename = GetChildWithName(go5, "name");
                                                        thenametext = thename.GetComponent<TMP_Text>();
                                                        thescore = GetChildWithName(go5, "score");
                                                        thescoretext = thescore.GetComponent<TMP_Text>();
                                                        thenametext.text = name;
                                                        thescoretext.text = points.ToString();
                                                        break;

                                                    default:
                                                        break;
                                                }

                                                count += 1;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Debug.LogWarning(DBTask1.Exception);
                                    }

                                }
                                else
                                {
                                    Debug.LogWarning(message: $"Leaderboard is empty");
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning(message: $"User {name} does not exist yet...");
                var DBTask2 = DBreference.Child("users").Child(NameConversion(name)).Child("name").SetValueAsync(name);
                yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);
                if (DBTask2.Exception == null)
                {
                    DBTask2 = DBreference.Child("users").Child(NameConversion(name)).Child("points").SetValueAsync(score);
                    yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);
                    if (DBTask2.Exception == null)
                    {
                        int count = 1;
                        DBTask = FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync();

                        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

                        if (DBTask.Exception != null)
                        {
                            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
                        }
                        else
                        {
                            if (DBTask.Result.GetValue(true) != null)
                            {
                                var DBTask1 = DBreference.Child("users").OrderByChild("points").GetValueAsync();

                                yield return new WaitUntil(predicate: () => DBTask1.IsCompleted);

                                if (DBTask1.Exception == null)
                                {
                                    DataSnapshot snapshot = DBTask1.Result;
                                    foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
                                    {
                                        if (count <= N)
                                        {
                                            name = childSnapshot.Child("name").Value.ToString();
                                            int points = int.Parse(childSnapshot.Child("points").Value.ToString());
                                            /*                            var playerdetails = new Dictionary<string, int>();
                                                                        playerdetails.Add(name, points);
                                                                        highscore.Add(playerdetails);*/
                                            Debug.Log($"Player {name}'s high score is {points}");

                                            switch (count)
                                            {
                                                case 1:
                                                    go1.SetActive(true);
                                                    GameObject thename = GetChildWithName(go1, "name");
                                                    TMP_Text thenametext = thename.GetComponent<TMP_Text>();
                                                    GameObject thescore = GetChildWithName(go1, "score");
                                                    TMP_Text thescoretext = thescore.GetComponent<TMP_Text>();
                                                    thenametext.text = name;
                                                    thescoretext.text = points.ToString();
                                                    break;

                                                case 2:
                                                    go2.SetActive(true);
                                                    thename = GetChildWithName(go2, "name");
                                                    thenametext = thename.GetComponent<TMP_Text>();
                                                    thescore = GetChildWithName(go2, "score");
                                                    thescoretext = thescore.GetComponent<TMP_Text>();
                                                    thenametext.text = name;
                                                    thescoretext.text = points.ToString();
                                                    break;

                                                case 3:
                                                    go3.SetActive(true);
                                                    thename = GetChildWithName(go3, "name");
                                                    thenametext = thename.GetComponent<TMP_Text>();
                                                    thescore = GetChildWithName(go3, "score");
                                                    thescoretext = thescore.GetComponent<TMP_Text>();
                                                    thenametext.text = name;
                                                    thescoretext.text = points.ToString();
                                                    break;

                                                case 4:
                                                    go4.SetActive(true);
                                                    thename = GetChildWithName(go4, "name");
                                                    thenametext = thename.GetComponent<TMP_Text>();
                                                    thescore = GetChildWithName(go4, "score");
                                                    thescoretext = thescore.GetComponent<TMP_Text>();
                                                    thenametext.text = name;
                                                    thescoretext.text = points.ToString();
                                                    break;

                                                case 5:
                                                    go5.SetActive(true);
                                                    thename = GetChildWithName(go5, "name");
                                                    thenametext = thename.GetComponent<TMP_Text>();
                                                    thescore = GetChildWithName(go5, "score");
                                                    thescoretext = thescore.GetComponent<TMP_Text>();
                                                    thenametext.text = name;
                                                    thescoretext.text = points.ToString();
                                                    break;

                                                default:
                                                    break;
                                            }

                                            count += 1;
                                        }
                                    }
                                }
                                else
                                {
                                    Debug.LogWarning(DBTask1.Exception);
                                }

                            }
                            else
                            {
                                Debug.LogWarning(message: $"Leaderboard is empty");
                            }
                        }
                    }
                    else
                    {

                    }

                }
                else
                {

                }
            }
        }
    }

    /// <summary>
    /// NOT IN USED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    /// </summary>

    public void test()
    {
        string name = "Ernest Ang";
        CreateNewUser(name);
        UpdateUserScore(name, 150);
        Debug.Log(retreivePersonalHighScore(name));
        //name1.SetText("Ernest");
        //score1.SetText(retreivePersonalHighScore(name).ToString());

        name = "Random Player 1";
        CreateNewUser(name);
        UpdateUserScore(name, 5);
        Debug.Log(retreivePersonalHighScore(name));
        // name2.SetText("Johny");
        // score2.SetText(retreivePersonalHighScore(name).ToString());

        name = "Random Player 2";
        CreateNewUser(name);
        UpdateUserScore(name, 120);
        Debug.Log(retreivePersonalHighScore(name));
        //name3.SetText(name);
        // score3.SetText(retreivePersonalHighScore(name).ToString());

        name = "Jeremiah";
        CreateNewUser(name);
        UpdateUserScore(name, 90);
        //name4.SetText(name);
        // score4.SetText(retreivePersonalHighScore(name).ToString());
        Debug.Log(retreivePersonalHighScore(name));
        Debug.Log("name set successfully");

        name = "you";
        CreateNewUser(name);
        UpdateUserScore(name, 20);
        //name4.SetText(name);
        // score4.SetText(retreivePersonalHighScore(name).ToString());
        Debug.Log(retreivePersonalHighScore(name));
        Debug.Log("name set successfully");

        retreiveTopNHighScore(4);
    }

    public void CreateNewUser(string new_user)
    {
        if (FirebaseDatabase.DefaultInstance.GetReference("users").Child(NameConversion(new_user)).GetValueAsync().Result.Value == null)
        {
            DBreference.Child("users").Child(NameConversion(new_user)).Child("name").SetValueAsync(new_user);
        }
        else
        {
            Debug.Log($"User already exists {NameConversion(new_user)}");
        }
    }

    public void UpdateUserScore(string new_user, int points)
    {
        if (FirebaseDatabase.DefaultInstance.GetReference("users").Child(NameConversion(new_user)).GetValueAsync().Result.Value != null)
        {
            if (FirebaseDatabase.DefaultInstance.GetReference("users").Child(NameConversion(new_user)).Child("points").GetValueAsync().Result.Value == null)
            {
                DBreference.Child("users").Child(NameConversion(new_user)).Child("points").SetValueAsync(points);
            }
            else
            {
                DataSnapshot snapshot = DBreference.Child("users").Child(NameConversion(new_user)).Child("points").GetValueAsync().Result;
                if (int.Parse(snapshot.Value.ToString()) >= points)
                {
                    Debug.Log($"Points earned {points} is less or equal than personal high score of {snapshot.Value.ToString()}!");
                }
                else
                {
                    DBreference.Child("users").Child(NameConversion(new_user)).Child("points").SetValueAsync(points);
                }

            }
        }
        else
        {
            Debug.Log($"No user {NameConversion(new_user)} found");
        }
    }

    public int? retreivePersonalHighScore(string new_user)
    {
        if (FirebaseDatabase.DefaultInstance.GetReference("users").Child(NameConversion(new_user)).GetValueAsync().Result.Value != null)
        {
            if (FirebaseDatabase.DefaultInstance.GetReference("users").Child(NameConversion(new_user)).Child("points").GetValueAsync().Result.Value == null)
            {
                return 0;
            }
            else
            {
                DataSnapshot snapshot = DBreference.Child("users").Child(NameConversion(new_user)).Child("points").GetValueAsync().Result;
                return int.Parse(snapshot.Value.ToString());
            }
        }
        else
        {
            Debug.Log($"No user {NameConversion(new_user)} found");
            return null;
        }
    }

    public List<Dictionary<string, int>> retreiveTopNHighScore(int N)
    {
        int count = 0;
        var highscore = new List<Dictionary<string, int>>();
        if (FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync().Result.Value != null)
        {
            DataSnapshot snapshot = DBreference.Child("users").OrderByChild("points").GetValueAsync().Result;

            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                if (count < N)
                {
                    string name = childSnapshot.Child("name").Value.ToString();
                    int points = int.Parse(childSnapshot.Child("points").Value.ToString());
                    var playerdetails = new Dictionary<string, int>();
                    playerdetails.Add(name, points);
                    highscore.Add(playerdetails);
                    //Debug.Log($"Player {name}'s high score is {points}");
                    count += 1;
                }
            }

            return highscore;
        }
        else
        {
            Debug.Log("No users found at all, database is empty!");
        }
        return null;
    }

    private string NameConversion(string name, bool toRemoveSpace = true) 
    {
        if (toRemoveSpace)
        {
            return name.Replace(" ", "_");
        }
        return name.Replace("_", " ");
    }

    GameObject GetChildWithName(GameObject fromGameObject, string withName)
    {
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts)
        {
            if (t.gameObject.name == withName) return t.gameObject;
        }
        return null;
    }

}