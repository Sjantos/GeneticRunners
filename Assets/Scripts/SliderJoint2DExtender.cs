using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderJoint2DExtender : MonoBehaviour
{
    [SerializeField] float maxDistance = 15f;
    [SerializeField] SliderJoint2D joint;

    [SerializeField] float changeTime = 0.25f;
    //[SerializeField] bool shorty = false;
    [SerializeField] bool shorten = false;

    private float timer = 0f;

    public void Init(SliderJoint2D slider, float timeChanger)
    {
        joint = slider;
        changeTime = timeChanger;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= changeTime)
        {
            timer = 0f;
            joint.SetMotorSpeed(joint.motor.motorSpeed * -1f);

            shorten = !shorten;
        }
        else
        {
            if (Vector3.Distance(transform.position, joint.connectedBody.transform.position) >= maxDistance)
            {
                timer = 0f;
                joint.SetMotorSpeed(joint.motor.motorSpeed * -1f);

                shorten = !shorten;
            }
        }
    }
}
