using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuscleJoint : MonoBehaviour
{
    public Rigidbody2D rb1;
    public Rigidbody2D rb2;
    public float timeChange = 0.2f;
    public bool canWork = false;
    public float force = 5f;
    public float strength = 0.3f;

    private float timer = 0f;
    [SerializeField] bool shorten = false;

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if(timer >= timeChange)
        {
            timer = 0f;
            if(shorten)
            {

            }
            else
            {

            }
            shorten = !shorten;
        }
        else
        {

        }
    }
}
