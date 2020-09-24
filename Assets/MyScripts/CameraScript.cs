using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private NetworkScript network;
    void Start()
    {
        network = NetworkScript.Insance;

        if(network.host)
        {
            Debug.Log("This is the Host!");
        }
        else
        {
            Debug.Log("This is the remote player flipping Camera");
            
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 180);
        }
    }
}
