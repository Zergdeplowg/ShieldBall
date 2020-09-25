using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkScript : MonoBehaviour
{
    // Start is called before the first frame update
    public static NetworkScript Insance;
    private UdpClient Client;
    public bool host;
    public string ip;
    public int port;
    private IPEndPoint endPoint;
    public bool connected = false;
    private bool listening = false;
    public delegate void HandlePlayerPacket(PlayerPacket packet);
    public HandlePlayerPacket handlePlayerPacket;
    private void Awake()
    {
        if(Insance == null)
        {
            DontDestroyOnLoad(gameObject);
            Insance = this;
        }
        else if(Insance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ConnectToServer()
    {
        Client = new UdpClient();
        Client.Connect(ip, port);
        endPoint = new IPEndPoint(System.Net.IPAddress.Parse(ip), port);
        Client.Client.ReceiveTimeout = 3;
        send<string>("Hello, I would Like to connect!");
        listening = true;
    }

    public void HostServer()
    {
        endPoint = new IPEndPoint(IPAddress.Any, port);
        Client = new UdpClient(endPoint);
        Client.Client.ReceiveTimeout = 3;
        listening = true;
    }

    public void send<T>(T Data)
    {
        Packet packet = Packet.CreatePacket<T>(Data);
        if (host)
        {
            string json = JsonUtility.ToJson(packet);
            var data = Encoding.ASCII.GetBytes(json);
            Client.Send(data, data.Length, endPoint);
        }
        else
        {
            string json = JsonUtility.ToJson(packet);
            var data = Encoding.ASCII.GetBytes(json);
            Client.Send(data, data.Length);
        }
    }

    public Packet Receive()
    {
        string json = "";
        try
        {
            var result = Client.Receive(ref endPoint);
            json = Encoding.ASCII.GetString(result, 0, result.Length);
        }
        catch(SocketException ex)
        {
            //Debug.Log(ex.ErrorCode);
            /*connected = false;
            Client.Close();
            SceneManager.LoadScene("MainMenu");*/
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        return JsonUtility.FromJson<Packet>(json);
    }

    public void Update()
    {
        if(listening)
        {
            Packet packet = Receive();

            if (packet != null)
            {
                switch (packet.type)
                {
                    case "String":
                        {
                            Debug.Log($"Message: {packet.getData<string>()}");
                            connected = true;
                            if (host)
                            {
                                send<string>("Hello, you are connected!");
                            }
                            break;
                        }
                    case "PlayerPacket":
                        {
                            handlePlayerPacket(packet.getData<PlayerPacket>());
                            break;
                        }
                }
            }
            if(listening && !connected && !host)
            {
                send<string>("Hello, I would like to connect");
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (Client != null)
            Client.Close();
    }
}
