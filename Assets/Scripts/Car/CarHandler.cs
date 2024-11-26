using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CarHandler : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;

    float accelerationMultiplier = 3;
    float breakaMultiplier = 15;
    float steeringMultiplier = 5;

    Vector2 input = Vector2.zero;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (input.y > 0)
            Accelerate();
        else
            rb.drag = 0.2f;

        if (input.y < 0)
            Break();

            Steer();
    }

    void Accelerate()
    {
        rb.drag = 0;
        rb.AddForce(transform.forward * accelerationMultiplier * input.y);  
    }

    void Break()
    {
        if(rb.velocity.z <= 0)
            return;
        rb.AddForce(transform.forward * breakaMultiplier * input.y);  
    }

    void Steer()
    {
     if (Mathf.Abs(input.x) > 0)
        {
            rb.AddForce(rb.transform.right * steeringMultiplier * input.x);
        }
    }

    public void SetInput(Vector2 inputVector)
    {
        inputVector.Normalize();
        input = inputVector;
    }
}
