using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject pauseButtons;
    public GameObject settings;
    private bool isPaused=false;

    private void Start()
    {
        ResumeGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
                BackFromSettings();
            }
        }       
    }

    void PauseGame()
    {
        pauseMenu.SetActive(true);
        isPaused = true;
    }

    void ResumeGame()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    public void Settings()
    {
        settings.SetActive(true);
        pauseButtons.SetActive(false);       
    }

    public void BackFromSettings()
    {
        settings.SetActive(false);
        pauseButtons.SetActive(true);    
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
        PhotonNetwork.Disconnect();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
