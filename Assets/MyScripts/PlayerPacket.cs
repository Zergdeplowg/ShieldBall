using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerPacket
{
    public float posX, posY, posZ, rotX, rotY, rotZ, shotP;
    public bool loading, fire;

    public PlayerPacket(float pX, float pY, float pZ, float rX, float rY, float rZ, float sP, bool l, bool f)
    {
        posX = pX;
        posY = pY;
        posZ = pZ;
        rotX = rX;
        rotY = rY;
        rotZ = rZ;
        shotP = sP;
        loading = l;
        fire = f;
    }
}
