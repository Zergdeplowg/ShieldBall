using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RemoteController : MonoBehaviour
{
    private NetworkScript network;
    private Movement movement;
    private Task listener = null;
    void Start()
    {
        movement = GetComponent<Movement>();
        network = NetworkScript.Insance;
    }

    public void Update()
    {
        PlayerPacket packet = network.Receive();
        if (packet != null)
        {
            transform.position = new Vector3(packet.posX, packet.posY, packet.posZ);
            transform.rotation = Quaternion.Euler(new Vector3(packet.rotX, packet.rotY, packet.rotZ));
            if (packet.fire)
            {
                fire(packet.shotP);
            }
            else if (packet.loading)
            {
                load(packet.shotP);
            }
        }
    }

    private void load(float power)
    {
        Debug.Log($"Loading shot at power {power}");
    }

    private void fire(float power)
    {
        Debug.Log($"Fireing shot at power {power}");
    }

}