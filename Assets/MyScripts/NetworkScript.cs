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
        sendConnectionPacket();
    }

    public void HostServer()
    {
        endPoint = new IPEndPoint(IPAddress.Any, port);
        Client = new UdpClient(endPoint);
    }

    public void sendPlayerPacket(PlayerPacket packet)
    {
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

    public void sendConnectionPacket()
    {
        if (host)
        {
            string json = "Hello, you are connected";
            var data = Encoding.ASCII.GetBytes(json);
            Client.Send(data, data.Length, endPoint);
        }
        else
        {
            string json = "Hello, I would like to connect";
            var data = Encoding.ASCII.GetBytes(json);
            Client.Send(data, data.Length);
        }
    }

    public async Task<PlayerPacket> ReceiveAsync()
    {
        var result = await Client.ReceiveAsync();
        string json = Encoding.ASCII.GetString(result.Buffer, 0, result.Buffer.Length);
        return JsonUtility.FromJson<PlayerPacket>(json);
    }

    public PlayerPacket Receive()
    {
        string json = "";
        try
        {
            var result = Client.Receive(ref endPoint);
            json = Encoding.ASCII.GetString(result, 0, result.Length);
        }
        catch(SocketException ex)
        {
            Debug.Log(ex);
            connected = false;
            Client.Close();
            SceneManager.LoadScene("MainMenu");
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        return JsonUtility.FromJson<PlayerPacket>(json);
    }

    public async Task ReceiveConnection()
    {
        var result = await Client.ReceiveAsync();
        string json = Encoding.ASCII.GetString(result.Buffer, 0, result.Buffer.Length);
        if (host)
        {
            endPoint = result.RemoteEndPoint;
        }
        connected = true;
        Debug.Log(json);
        Client.Client.ReceiveTimeout = 1;
    }

    private void OnApplicationQuit()
    {
        if (Client != null)
            Client.Close();
    }
}
