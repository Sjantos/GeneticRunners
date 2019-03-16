using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Node : MonoBehaviour
{
    [SerializeField] SliderJoint2D[] joints;
    [SerializeField] SliderJoint2DExtender[] extenders;

    public Rigidbody2D Rigidbody2D {  get { return GetComponent<Rigidbody2D>(); } }

    public void SetUp(float friction, Muscle[] muscles)
    {
        joints = new SliderJoint2D[muscles.Length];
        extenders = new SliderJoint2DExtender[muscles.Length];
        for (int i = 0; i < muscles.Length; i++)
        {
            joints[i] = gameObject.AddComponent<SliderJoint2D>();
            joints[i].connectedBody = muscles[i].connectedObj;
            joints[i].motor = new JointMotor2D()
            {
                motorSpeed = 5f,
                maxMotorTorque = 100000f
            };
            extenders[i] = gameObject.AddComponent<SliderJoint2DExtender>();
            var limits = joints[i].limits;
            limits.min = muscles[i].minLength;
            limits.max = muscles[i].maxLength;
            joints[i].angle = muscles[i].Angle;
            joints[i].limits = limits;
            joints[i].useLimits = true;
            joints[i].useMotor = true;
            joints[i].autoConfigureAngle = false;
        }
        gameObject.SetActive(true);
    }

    public void Reset()
    {
        gameObject.SetActive(false);
        foreach (var component in GetComponents<SliderJoint2D>())
        {
            DestroyImmediate(component);
        }
        foreach (var component in GetComponents<SliderJoint2DExtender>())
        {
            DestroyImmediate(component);
        }
        joints = null;
        extenders = null;
    }
}
