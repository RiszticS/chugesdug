using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settings;

    private void Singleplayer()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.OfflineMode = true;
        SceneManager.LoadScene("Loading");
    }

    private void Multiplayer()
    {
        PhotonNetwork.OfflineMode = false;
        SceneManager.LoadScene("Loading");
    }

    private void Settings()
    {
        mainMenu.SetActive(false);
        settings.SetActive(true);
    }

    private void BackToMainMenu()
    {
        settings.SetActive(false);
        mainMenu.SetActive(true);
    }

    private void Exit()
    {
        Application.Quit();
    }
}
