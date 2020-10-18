using System.Collections;
using System.Collections.Generic;
//using TMPro;
using UnityEngine;

public class VehicleController1 : MonoBehaviour
{
    private float verticalInput;
    private float horizontalInput;
    private Rigidbody chassis;
    public float forceImpulse;
    public float torqueImpulse;

    VehicleController1()
    {
        forceImpulse = 0.0f;
        torqueImpulse = 0.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        verticalInput = 0.0f;
        horizontalInput = 0.0f;
        chassis = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GetKeys();   
    }
    void FixedUpdate()
    {
        UpdateController();
    }

    void UpdateController()
    {
        if(Mathf.Abs(verticalInput) > 0.0f)
        {
            // Calculate the impulse
            Vector3 vimpulse = transform.forward * forceImpulse * verticalInput;
            // Draw the impulse 
            Utils.DrawArrow(transform.position, vimpulse, Color.blue);
            // Apply the impulse
            chassis.AddForce(vimpulse, ForceMode.Impulse);
        }

        if(Mathf.Abs(horizontalInput) > 0.0f)
        {
            Vector3 himpulse = transform.up * forceImpulse * verticalInput;

            Utils.DrawArrow(transform.position, himpulse, Color.green);

            chassis.AddTorque(himpulse, ForceMode.Impulse);
        }
    }

    // Get use input
    void GetKeys()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
    }

    //Collision
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Box")
        {
            Debug.Log("CollisionSuccess");
           // ParticleSystem part = GetComponent<ParticleSystem>;
            Destroy(GameObject.FindWithTag("Box"));
            
        }
    }
}
