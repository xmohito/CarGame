using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarHandler : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    Transform gameModel;
    [SerializeField]
    MeshRenderer carMeshRender;

    //Max values
    float maxForwardVelocity = 30;
    float maxSteerVelocity = 2;

    //Multipliers
    float accelerationMultiplier = 3;
    float breakaMultiplier = 15;
    float steeringMultiplier = 5;


    Vector2 input = Vector2.zero;

    int _EmissionColor = Shader.PropertyToID("_EmissionColor");
    Color emissiveColor = Color.white;
    float emissiveColorMultiplier = 0f;
    void Start()
    {
        
    }

    void Update()
    {
        gameModel.transform.rotation = Quaternion.Euler(0, rb.velocity.x * 5, 0);

        if (carMeshRender != null)
        {
            float desiredCarEmissiveColorMultiplier = 0f;
            if (input.y < 0)
                desiredCarEmissiveColorMultiplier = 4.0f;

            emissiveColorMultiplier = Mathf.Lerp(emissiveColorMultiplier, desiredCarEmissiveColorMultiplier, Time.deltaTime * 6);
            carMeshRender.material.SetColor(_EmissionColor, emissiveColor * emissiveColorMultiplier);
        }
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

        if(rb.velocity.z <= 0)
            rb.velocity = Vector3.zero;
    }

    void Accelerate()
    {
        rb.drag = 0;
        if(rb.velocity.z >= maxForwardVelocity)
            return;

        rb.AddForce(rb.transform.forward * accelerationMultiplier * input.y);  
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
            float speedBasedSteerLimit = rb.velocity.z / 5.0f;
            speedBasedSteerLimit = Mathf.Clamp01(speedBasedSteerLimit);
            rb.AddForce(rb.transform.right * steeringMultiplier * input.x * speedBasedSteerLimit);

            float normalizedX = rb.velocity.x / maxSteerVelocity;

            normalizedX = Mathf.Clamp(normalizedX, -1.0f, 1.0f);

            rb.velocity = new Vector3(normalizedX * maxSteerVelocity, 0, rb.velocity.z);
        }
        else
        {
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0, 0, rb.velocity.z), Time.fixedDeltaTime * 3);
        }
    }

    public void SetInput(Vector2 inputVector)
    {
        inputVector.Normalize();
        input = inputVector;
    }
}
