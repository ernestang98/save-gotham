using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public panel currentPanel = null;
    public GameObject InstructionMenu;
    public GameObject LoseMenu;
    public GameObject WinMenu;
    public GameObject PauseMenu;
    public GameObject vrRig;
    public GameObject MainMenuCanvas;

    private List<panel> panelHistory = new List<panel>();

    private void Start()
    {
        SetupPanels();
        if (PlayerPrefs.GetInt("isRestart") == 1)
        {
            MainMenuCanvas.SetActive(false);
            InstructionMenu.SetActive(true);
        }
    }



    private void SetupPanels()
    {
        panel[] panels = GetComponentsInChildren<panel>();

        foreach(panel panel in panels)
        {
            panel.Setup(this);

            currentPanel.Show();
        }
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown("p"))
        {
            PauseMenu.SetActive(true);
            Time.timeScale = 0;
            AudioSource[] audios = FindObjectsOfType<AudioSource>();

            foreach (AudioSource a in audios)
            {
                a.Pause();
            }
            vrRig.GetComponent<ActionBasedContinuousTurnProvider>().enabled = false;
            vrRig.GetComponent<ActionBasedContinuousMoveProvider>().enabled = false;

            Debug.Log("p pressed");
        } 
        else if ((InstructionMenu.activeInHierarchy) || (LoseMenu.activeInHierarchy) || (WinMenu.activeInHierarchy) || (MainMenuCanvas.activeInHierarchy))
        {
            PauseMenu.SetActive(false);
            Time.timeScale = 0;
            AudioSource[] audios = FindObjectsOfType<AudioSource>();

            foreach (AudioSource a in audios)
            {
                a.Pause();
            }
            vrRig.GetComponent<ActionBasedContinuousTurnProvider>().enabled = false;
            vrRig.GetComponent<ActionBasedContinuousMoveProvider>().enabled = false;
        }

       
    }

    public void Resume()
    {
        InstructionMenu.SetActive(false);
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
        vrRig.GetComponent<ActionBasedContinuousTurnProvider>().enabled = true;
        vrRig.GetComponent<ActionBasedContinuousMoveProvider>().enabled = true;
        AudioSource[] audios = FindObjectsOfType<AudioSource>();

        foreach (AudioSource a in audios)
        {
            a.Play();
        }
    }

    public void GoToPrevious()
    {
        if (panelHistory.Count == 0)
        {
            //SceneManager.LoadScene("MainMenu");
            return;
        }

        int lastIndex = panelHistory.Count - 1;
        SetCurrent(panelHistory[lastIndex]);
        panelHistory.RemoveAt(lastIndex);
           
    }

    public void SetCurrentWithHistory(panel newPanel)
    {
        panelHistory.Add(currentPanel);
        SetCurrent(newPanel);
    }

    private void SetCurrent(panel newPanel)
    {
        currentPanel.Hide();

        currentPanel = newPanel;
        currentPanel.Show();
    }

    public void Restart()
    {
        PlayerPrefs.SetInt("isRestart", 1);
        SceneManager.LoadScene("VR Game");
    }

    public void MainMenu()
    {
        PlayerPrefs.SetInt("isRestart", 0);
        SceneManager.LoadScene("VR Game");
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt("isRestart", 0);
    }

}

