using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JointsExtensions
{
    public static void SetMotorSpeed(this SliderJoint2D sliderjoint2D, float speed)
    {
        var moto = sliderjoint2D.motor;
        moto.motorSpeed = speed;
        sliderjoint2D.motor = moto;
    }
}
