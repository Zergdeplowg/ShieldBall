using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[System.Serializable]
public class Packet
{
    public string type;
    public byte[] data;

    public static Packet CreatePacket<T>(T data)
    {
        return new Packet(typeof(T).Name,Encoding.ASCII.GetBytes(JsonUtility.ToJson(data)));
    }
    private Packet(string T,byte[] json)
    {
        type = T;
        data = json;
    }

    public T getData<T>()
    {
        return JsonUtility.FromJson<T>(Encoding.ASCII.GetString(data,0,data.Length));
    }

}
