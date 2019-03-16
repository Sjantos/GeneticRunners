using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderJoint2DExtender : MonoBehaviour
{
    [SerializeField] SliderJoint2D joint;

    [SerializeField] float changeTime = 0.25f;
    [SerializeField] bool shorten = true;

    private JointMotor2D motor;
    private float timer = 0f;

    private void Start()
    {
        joint = GetComponent<SliderJoint2D>();
        motor = joint.motor;
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if(timer >= changeTime)
        {
            timer = 0f;
            if (shorten)
            {
                motor.motorSpeed *= -1f;
                joint.motor = motor;
            }
            else
            {
                motor.motorSpeed *= -1f;
                joint.motor = motor;
            }
            shorten = !shorten;
        }
    }
}
