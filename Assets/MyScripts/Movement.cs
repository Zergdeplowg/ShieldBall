using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public Rigidbody myRigidbody;
    public float UAxis = 0.0f;
    public float RAxis = 0.0f;
    public Quaternion rotation = Quaternion.identity;
    public bool remote = false;
    private bool localController;
    private Vector3 velocity;
    private NetworkScript network;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        network = NetworkScript.Insance;

        if((remote & network.host) || (!remote & !network.host))
        {
            gameObject.AddComponent<RemoteController>();
            localController = false;
        }
        else
        {
            gameObject.AddComponent<LocalController>();
            localController = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(localController)
            velocity = new Vector3(RAxis, 0, UAxis)* 10;
    }

    private void FixedUpdate()
    {
        if (localController)
        {
            myRigidbody.velocity = velocity;
            myRigidbody.transform.rotation = rotation;
        }
    }
}
