using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System.Data;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_InputField guestusernameInput;
    public TMP_Text buttonText;
    public GameObject guestSection;
    public GameObject loginSection;

    void Start()
    {
        if (PhotonNetwork.OfflineMode)
        {
            buttonText.text = "Start Game";
        }
        else
        {
            buttonText.text = "Connect";
        }
    }

    public void ShowGuest()
    {
        loginSection.SetActive(false);
        guestSection.SetActive(true);
    }

    public void ShowLogin()
    {
        guestSection.SetActive(false);
        loginSection.SetActive(true);
    }

    public void OnClickConnect()
    {
        if (guestusernameInput.text.Length>1)
        {
            PhotonNetwork.NickName = guestusernameInput.text;
            if (!PhotonNetwork.OfflineMode)
            {
                PhotonNetwork.AutomaticallySyncScene = true;
                PhotonNetwork.ConnectUsingSettings();
                buttonText.text = "Connecting...";
            }
            else
            {
                SceneManager.LoadScene("SingleplayerLobby");
            }           
        }
    }

    public void OnClickLogin()
    {
        string username = usernameInput.text;
        string psw = passwordInput.text;
        string s = "CALL `Login`(@p0, @p1);";
        MySqlConnection c1 = SqlConnector.ConnectDB();
        MySqlCommand ms = new MySqlCommand(s, c1);
        ms.Parameters.AddWithValue("@p0", username);
        ms.Parameters.AddWithValue("@p1", psw);
        ms.Parameters["@p0"].Direction = ParameterDirection.Input;
        ms.Parameters["@p1"].Direction = ParameterDirection.Input;

        MySqlDataReader reader = ms.ExecuteReader();

        if (reader.Read())
        {         
            Debug.Log("Sikeres");
            PhotonNetwork.NickName = reader.GetValue(0).ToString();
            buttonText.text = "Connecting...";
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();    
        }
        else
        {
            Debug.Log("Sikertelen");
        }
        SqlConnector.DisconnectDB(ref c1);
    }


    public override void OnConnectedToMaster()
    {
            SceneManager.LoadScene("Lobby");     
    }
}
