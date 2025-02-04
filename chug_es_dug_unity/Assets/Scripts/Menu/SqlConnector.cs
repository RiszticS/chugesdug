using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;

public class SqlConnector : MonoBehaviour
{
    const string server = "127.0.0.1";
    //const uint port = 3306;
    const string user = "root";
    const string psw = "";
    const string db = "chuganddug";


    public static MySqlConnection ConnectDB()
    {
        MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();       
        builder.Server = server;
      //builder.Port = port;
        builder.UserID = user;
        builder.Password = psw;
        builder.Database = db;
        builder.SslMode = MySqlSslMode.None;
        string connString = builder.ToString();
        Debug.Log(connString);   

        MySqlConnection Con = new MySqlConnection(connString);
      
        try
        {
            Con.Open();
        }
        catch (MySql.Data.MySqlClient.MySqlException ex)
        {
            Debug.Log("hiba: "+ex.Message+"  "+ ex.Number);
            throw;
        }       
        return Con;
    }

    public static void DisconnectDB(ref MySqlConnection Con)
    {
        Con.Close();
    }
}
