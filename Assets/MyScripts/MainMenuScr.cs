using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScr : MonoBehaviour
{
    private string ipAddress = "127.0.0.1";
    private int portNum = 3980;
    public Text inputField;
    private NetworkScript network;
    public GameObject buttonPannel;
    public GameObject connectingPannel;
    private bool loaded = false;

    public void Start()
    {
        network = NetworkScript.Insance;
    }
    public void Host()
    {
        Debug.Log("The host button was pushed");
        network.port = portNum;
        network.host = true;
        network.ip = ipAddress;
        network.HostServer();
        waitForConnection();
    }
    public void Connect()
    {
        Debug.Log($"The connect button was pushed. The IP address Entered was {ipAddress}");

        network.port = portNum;
        network.host = false;
        network.ip = ipAddress;
        network.ConnectToServer();
        waitForConnection();
    }

    public void UpdateIP()
    {
        ipAddress = inputField.text;
    }

    private void waitForConnection()
    {
        connectingPannel.SetActive(true);
        buttonPannel.SetActive(false);
    }

    private void loadLevel()
    {
        Debug.Log("Load Level called!");
        SceneManager.LoadSceneAsync("BasicArena",LoadSceneMode.Single);
    }

    public void Update()
    {
        if (network.connected && !loaded)
        {
            loadLevel();
            loaded = true;
        }
    }
}
