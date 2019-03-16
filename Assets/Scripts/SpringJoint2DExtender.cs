using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpringJoint2D))]
public class SpringJoint2DExtender : MonoBehaviour
{
    [SerializeField] SpringJoint2D joint;
    [SerializeField] Transform otherObject;
    [SerializeField] float additionalExtend = 0.1f;
    [SerializeField] float additionalShrink = 0.1f;

    [SerializeField] float changeTime = 0.25f;
    [SerializeField] bool shorten = true;
    [SerializeField] float minLength = 1f;
    [SerializeField] float maxLength = 2f;
    
    private float timer = 0f;

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if(timer >= changeTime)
        {
            timer = 0f;
            if (shorten)
            {
                transform.position += Vector3.ClampMagnitude((otherObject.position - transform.position), additionalShrink / 2f);
                otherObject.position += Vector3.ClampMagnitude((transform.position - otherObject.position), additionalShrink / 2f);
            }
            else
            {
                transform.position += Vector3.ClampMagnitude((transform.position - otherObject.position), additionalExtend / 2f);
                otherObject.position += Vector3.ClampMagnitude((otherObject.position - transform.position), additionalExtend / 2f);
            }
            shorten = !shorten;
        }
    }
}
