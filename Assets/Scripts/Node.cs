using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Node : MonoBehaviour
{
    //[SerializeField] SliderJoint2D[] joints;
    //[SerializeField] SliderJoint2DExtender[] extenders;
    public SliderJoint2D joint;
    public SliderJoint2DExtender extender;

    public Rigidbody2D Rigidbody2D {  get { return GetComponent<Rigidbody2D>(); } }

    public void SetUp(float friction, Muscle[] muscles)
    {
        //gameObject.SetActive(true);



        for (int i = 0; i < muscles.Length; i++)
        {
            joint = gameObject.AddComponent<SliderJoint2D>();
            joint.autoConfigureAngle = false;
            joint.autoConfigureConnectedAnchor = false;
            joint.useLimits = true;
            joint.useMotor = true;

            joint.connectedBody = muscles[i].connectedObj;
            joint.motor = new JointMotor2D()
            {
                motorSpeed = 5f,
                maxMotorTorque = 100000f
            };

            var limits = joint.limits;
            limits.min = muscles[i].minLength;
            limits.max = muscles[i].maxLength;
            joint.angle = muscles[i].Angle;
            joint.limits = limits;

            extender = gameObject.AddComponent<SliderJoint2DExtender>();
            //StartCoroutine(AutoAngleOff(joints[i]));
        }



        //var mat = new PhysicsMaterial2D();
        //mat.friction = friction;
        //GetComponent<Rigidbody2D>().sharedMaterial = mat;

        //joints = new SliderJoint2D[muscles.Length];
        //extenders = new SliderJoint2DExtender[muscles.Length];
        //for (int i = 0; i < muscles.Length; i++)
        //{
        //    joints[i] = gameObject.AddComponent<SliderJoint2D>();
        //    joints[i].connectedBody = muscles[i].connectedObj;
        //    joints[i].motor = new JointMotor2D()
        //    {
        //        motorSpeed = 5f,
        //        maxMotorTorque = 100000f
        //    };
        //    extenders[i] = gameObject.AddComponent<SliderJoint2DExtender>();
        //    var limits = joints[i].limits;
        //    limits.min = muscles[i].minLength;
        //    limits.max = muscles[i].maxLength;
        //    joints[i].angle = muscles[i].Angle;
        //    joints[i].limits = limits;
        //    joints[i].useLimits = true;
        //    joints[i].useMotor = true;
        //    joints[i].autoConfigureAngle = false;
        //    //StartCoroutine(AutoAngleOff(joints[i]));
        //}
        gameObject.SetActive(true);
    }

    private IEnumerator AutoAngleOff(SliderJoint2D joint)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        joint.autoConfigureAngle = false;
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
        joint = null;
        extender = null;
    }
}
