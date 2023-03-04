using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactParameters{

    public float pDistance; //in mm
    public Vector3 pDirection;
    public float pPerpendicularVelocity; //in mm/s
    public float pVelocity; //in mm/s

    public Vector3 previousPosition = Vector3.zero;

}
