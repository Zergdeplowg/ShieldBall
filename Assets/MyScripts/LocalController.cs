using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalController : MonoBehaviour
{
    private Movement movement;
    private Vector3 mouse;
    private Vector3 mouseWorld;
    private NetworkScript network;
    void Start()
    {
        movement = GetComponent<Movement>() as Movement;
        network = NetworkScript.Insance;
    }

    void Update()
    {
        if (network.host)
        {
            movement.UAxis = Input.GetAxis("Vertical");
            movement.RAxis = Input.GetAxis("Horizontal");
        }
        else
        {
            movement.UAxis = Input.GetAxis("Vertical") * -1;
            movement.RAxis = Input.GetAxis("Horizontal") * -1;
        }
        mouse = Input.mousePosition;
        mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 forward = Camera.main.ScreenToWorldPoint(new Vector3(mouse.x,mouse.y, 21.15f)) - transform.position;
        movement.rotation = Quaternion.LookRotation(new Vector3(forward.x,0,forward.z),Vector3.up);
        network.send(new PlayerPacket(transform.position.x, transform.position.y, transform.position.z, transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z, 0.0f, false, false));
    }

}
