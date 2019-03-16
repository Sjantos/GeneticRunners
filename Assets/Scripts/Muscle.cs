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
            var vec = (Vector2)connectedObj.gameObject.transform.position - (Vector2)baseObj.transform.position;
            return Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
        }
    }
}
