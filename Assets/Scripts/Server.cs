using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using System.Net;


public class Server : MonoBehaviour
{
    public int port = 6231;

    private List<ServerClient> clients;
    private List<ServerClient> disconnectList;

    private TcpListener server;
    private bool serverStarted;

    public void Init()
    {
        DontDestroyOnLoad(gameObject);
        clients = new List<ServerClient>();
        disconnectList = new List<ServerClient>();

        try
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();

            StartListening();
            serverStarted = true;
        }
        catch (Exception e)
        {
            Debug.Log("Socket Error: " + e.Message);
        }
    }

    private void Update()
    {
        if (!serverStarted)
            return;

        foreach (ServerClient c in clients)
        {
            /* Check if client is still connected? */
            if (!isConnected(c.tcp))
            {
                c.tcp.Close();
                disconnectList.Add(c);
                continue;
            }
            else
            {
                /* If Connected, check stream of every client */
                NetworkStream s = c.tcp.GetStream();
                if (s.DataAvailable)
                {
                    StreamReader reader = new StreamReader(s, true);
                    string data = reader.ReadLine();

                    if (data != null)
                    {
                        OnIncomingData(c, data);
                    }
                }
            }
        }

        for (int i = 0; i < disconnectList.Count - 1; i++)
        {
            /* Remove clients that are disconnected */
            clients.Remove(disconnectList[i]);
            disconnectList.RemoveAt(i);
        }
    }

    private void StartListening()
    {
        server.BeginAcceptTcpClient(AcceptTcpClient, server);
    }

    private void AcceptTcpClient(IAsyncResult ar)
    {
        TcpListener listener = (TcpListener) ar.AsyncState;

        string allUsers = "";
        foreach (ServerClient i in clients)
        {
            allUsers += i.clientName + '|';
        }

        ServerClient sc = new ServerClient(listener.EndAcceptTcpClient(ar));
        clients.Add(sc);

        /* listen again after adding tcp client to list */
        StartListening();

        //Debug Message
        //Debug.Log("Somebody has connected!");

        Broadcast("SWHO|" +allUsers, clients[clients.Count - 1]);
    }

    private bool isConnected(TcpClient c)
    {
        try
        {
            if (c != null && c.Client != null && c.Client.Connected)
            {
                if (c.Client.Poll(0, SelectMode.SelectRead))
                    return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);

                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }

    /* Function that Sends to the Client */
    private void Broadcast(string data, List<ServerClient> cl)
    {
        foreach (ServerClient sc in cl)
        {
            try
            {
                /* Write data to stream */
                StreamWriter writer = new StreamWriter(sc.tcp.GetStream()); // gets stream
                writer.WriteLine(data);                                     // write data to stream
                writer.Flush();                                             // flush writer
            }
            catch (Exception e)
            {
                /* If client is not reachable, debug message */
                Debug.Log("Write Error : " + e.Message);
            }
        }
    }

    /* Simple Overload for Broadcast() so it takes in a client rather than a list of client.
     * It just puts the single client, into a list, then calls Broadcast that takes in a list of client.
     */
    private void Broadcast(string data, ServerClient c)
    {
        List<ServerClient> sc = new List<ServerClient> { c };
        Broadcast(data, sc);
    }

    /* Function that Reads from the Client */
    private void OnIncomingData(ServerClient c, string data)
    {
        //Debug for incoming data (server)
        Debug.Log("Server: " + data);

        /* Process Data */
        //Debug.Log(c.clientName +" : " +data); //debug place holder
        //Debug.Log("Data: " + data);

        string[] aData = data.Split('|');

        switch (aData[0])
        {
            case "CWHO":
                c.clientName = aData[1];
                c.isHost = (aData[2] == "0") ? false : true; //tells server which client is host or not; 3rd index of aData is host value
                Broadcast("SCNN|" + c.clientName, clients);
                break;

            //case "CMOV":
            //    if (aData[2] == "1")
            //    Broadcast("SMOV|" + aData[1] +"|" +aData[2], clients);
            //    break;

            case "P1":
                Broadcast("SMOV|" + aData[1], clients[1]);
                break;

            case "P2":
                Broadcast("SMOV|" + aData[1], clients[0]);
                break;

            case "CJUMP":
                Broadcast("SJUMP", clients);
                break;

        }
    }
}

public class ServerClient
{
    public string clientName;
    public TcpClient tcp;
    public bool isHost;

    public ServerClient(TcpClient tcp)
    {
        this.tcp = tcp;
    }
}
