using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Node : MonoBehaviour
{
    public int MuscleCount = 0;
    [SerializeField] SliderJoint2D[] joints;
    [SerializeField] SliderJoint2DExtender[] extenders;

    public Rigidbody2D Rigidbody2D {  get { return GetComponent<Rigidbody2D>(); } }

    //private List<Rigidbody2D> connectedNodes = new List<Rigidbody2D>();
    private List<SliderJoint2D> addedJoints = new List<SliderJoint2D>();
    private List<SliderJoint2DExtender> jointsExtenders = new List<SliderJoint2DExtender>();

    private JointMotor2D defaultMotor = new JointMotor2D()
    {
        motorSpeed = 5f,
        maxMotorTorque = 100f
    };

    public void Enable()
    {
        joints = addedJoints.ToArray();
        extenders = jointsExtenders.ToArray();
        gameObject.SetActive(true);
    }

    public void AddConnectedNode(Node other)
    {
        var joint = gameObject.AddComponent<SliderJoint2D>();
        joint.autoConfigureAngle = false;
        joint.autoConfigureConnectedAnchor = false;
        addedJoints.Add(joint);
        joint.motor = new JointMotor2D() { motorSpeed = 5f, maxMotorTorque = 50f };
        joint.connectedBody = other.Rigidbody2D;
        //var distance = Vector3.Distance(transform.position, other.transform.position);
        joint.limits = new JointTranslationLimits2D()
        {
            min = 0f,
            max = 15f
        };
        joint.angle = JointAngle(transform.position, other.transform.position);
        joint.useLimits = true;
        joint.useMotor = true;

        var extender = gameObject.AddComponent<SliderJoint2DExtender>();
        jointsExtenders.Add(extender);
        extender.Init(joint);
    }

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
        MuscleCount = 0;
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

    private float JointAngle(Vector3 basePosition, Vector3 connectedPosition)
    {
        var vec = (Vector2)connectedPosition - (Vector2)basePosition;
        return Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
    }
}
