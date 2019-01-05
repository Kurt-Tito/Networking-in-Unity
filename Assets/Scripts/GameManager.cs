using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;           //for LocalIPAddress()
using System.Net.Sockets;   //for LocalIPAddress()
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { set; get; }

    public GameObject mainMenu;
    public GameObject serverMenu;
    public GameObject connectMenu;

    public GameObject serverPrefab;
    public GameObject clientPrefab;

    public InputField nameInput;

    private void Start()
    {
        Instance = this;
        //mainMenu.SetActive(false);
        serverMenu.SetActive(false);
        connectMenu.SetActive(false);
        DontDestroyOnLoad(gameObject);
    }

    /* Menu Connect Button */
    public void ConnectButton()
    {
        mainMenu.SetActive(false);
        connectMenu.SetActive(true);
    }

    public void HostButton()
    {
        /* Try and Create the Server */
        try
        {
            Server s = Instantiate(serverPrefab).GetComponent<Server>();
            s.Init();

            Client c = Instantiate(clientPrefab).GetComponent<Client>();
            c.isHost = true; //flag this client as host b.c. it is hosting the game

            c.clientName = nameInput.text;
            if (c.clientName == "")
                c.clientName = "Host";

            c.ConnectToServer("127.0.0.1", 6231);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        mainMenu.SetActive(false);
        serverMenu.SetActive(true);
    }

    /* Server Connect Button */
    public void ConnectToServerButton()
    {
        //initialize to text inside input field
        string hostAddress = GameObject.Find("HostInput").GetComponent<InputField>().text;

        //if textfield is empty, default it to localhost, 127.0.0.1
        if (hostAddress == "")
            hostAddress = "127.0.0.1";

        /* Try and create the Client */
        try
        {
            Client c = Instantiate(clientPrefab).GetComponent<Client>();
            c.clientName = nameInput.text;
            if (c.clientName == "")
                c.clientName = "Client";
            c.ConnectToServer(hostAddress, 6231);
            connectMenu.SetActive(false);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void BackButton()
    {
        mainMenu.SetActive(true);
        serverMenu.SetActive(false);
        connectMenu.SetActive(false);

        /* Check if Server Objects exists, if so, destroy on back */
        Server s = FindObjectOfType<Server>();
        if (s != null)
            Destroy(s.gameObject);

        /* Check if Client Objects exists, if so, destroy on back */
        Client c = FindObjectOfType<Client>();
        if (c != null)
            Destroy(c.gameObject);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Jump");
    }

}
