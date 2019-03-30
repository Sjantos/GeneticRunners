using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderJoint2DExtender : MonoBehaviour
{
    [SerializeField] SliderJoint2D joint;

    [SerializeField] float changeTime = 0.5f;
    //[SerializeField] bool shorty = false;
    [SerializeField] bool shorten = false;

    private float timer = 0f;

    public void Init(SliderJoint2D slider)
    {
        joint = slider;
    }

    private void Update()
    {
        //Debug.Log(joint.limitState);

        //if (shorten.HasValue)
        //{
        //    if (shorten.Value && joint.limitState == JointLimitState2D.LowerLimit)
        //    {
        //        Debug.Log("LowerLimit");
        //        joint.SetMotorSpeed(joint.motor.motorSpeed * -1f);
        //        shorten = false;
        //        return;
        //    }

        //    if (!shorten.Value && joint.limitState == JointLimitState2D.UpperLimit)
        //    {
        //        Debug.Log("IpperLimit");
        //        joint.SetMotorSpeed(joint.motor.motorSpeed * -1f);
        //        shorten = true;
        //        return;
        //    }
        //    shorty = shorten.Value;
        //}

        timer += Time.fixedDeltaTime;
        if (timer >= changeTime)
        {
            timer = 0f;
            joint.SetMotorSpeed(joint.motor.motorSpeed * -1f);

            shorten = !shorten;
        }
    }
}
