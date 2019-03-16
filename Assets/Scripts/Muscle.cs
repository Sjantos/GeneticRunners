using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muscle
{
    public GameObject baseObj;
    public Rigidbody2D connectedObj;
    public float minLength;
    public float maxLength;

    public float Angle
    {
        get
        {
            var pos = connectedObj.gameObject.transform.position;
            var vec = (Vector2)connectedObj.gameObject.transform.position - (Vector2)baseObj.transform.position;
            Debug.Log("Angle=" + (Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg));
            return Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
            
            return Vector2.SignedAngle(connectedObj.gameObject.transform.position, baseObj.transform.position);
        }
    }

    //public void SetUp(GameObject baseObj, Rigidbody2D connObj, float minLength, float maxLength)
    //{
    //    var limits = joint.limits;
    //    limits.min = minLength;
    //    limits.max = maxLength;
    //    joint.limits = limits;

    //    this.baseObj = baseObj;
    //    this.connectedObj = connObj.gameObject;
    //    this.joint.connectedBody = connObj;
    //}
}
