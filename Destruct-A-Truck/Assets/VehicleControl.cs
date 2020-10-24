using System.Collections;
using System.Collections.Generic;
//using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class VehicleControl : MonoBehaviour
{
    private float verticalInput;
    private float horizontalInput;
    private Rigidbody chassis;
    public float forceImpulse;
    public float torqueImpulse;
    public bool onGround;
    public float linearGrip;
    public float angularGrip;

    VehicleControl()
    {
        forceImpulse = 0.0f;
        torqueImpulse = 0.0f;
        linearGrip = 0.2f;
        angularGrip = 0.2f;
    }

    // Start is called before the first frame update
    void Start()
    {
        verticalInput = 0.0f;
        horizontalInput = 0.0f;
        onGround = false;
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
        // Get the vechicles directions in world space
        Vector3 currentForward = transform.forward;
        //Utils.DrawArrow(transform.position, currentForward * 2.0f, Color.green);
        Vector3 currentRight = transform.right;
        //Utils.DrawArrow(transform.position, currentRight * 2.0f, Color.black);
        Vector3 currentUp = transform.up;
        //Utils.DrawArrow(transform.position, currentUp * 2.0f, Color.cyan);

        ///Vector3 currentVelocity = chassis.velocity;
        // How much is our current velocity pointing in our current direction?
        ///float currentSpeed = Vector3.Dot(currentVelocity, currentForward);
        //Debug.Log("Current speed: " + currentSpeed);

        Vector3 angularVelocity = chassis.angularVelocity;
        float mass = chassis.mass;


        if (Mathf.Abs(verticalInput) > 0.0f)
        {
            // Calculate the impulse
            Vector3 vimpulse = transform.forward * forceImpulse * verticalInput;
            // Draw the impulse 
            Utils.DrawArrow(transform.position, vimpulse, Color.blue);
            // Apply the impulse
            chassis.AddForce(vimpulse, ForceMode.Impulse);
        }

        if (Mathf.Abs(horizontalInput) > 0.0f)
        {
            // Calculate the impulse
            Vector3 torque = transform.up * torqueImpulse * horizontalInput;
            // Draw the torque 
            Utils.DrawArrow(transform.position, torque, Color.red);
            // Apply the impulse
            chassis.AddTorque(torque, ForceMode.Impulse);
        }


        // Work out how fast we're sliding left/right
        // An compensate according to the grip property
        // Vector3.Dot(currentVelocity, currentRight);
        // How much is our current velocity pointing in our right direction? i.e. Are we sliding?
        ///float slideSpeed = Vector3.Dot(currentVelocity, currentRight);
        //Debug.Log("Slide speed: " + slideSpeed);
        // Apply an impulse to compensate for sliding
        ///Vector3 gripImp = currentRight * (-slideSpeed * mass * linearGrip);
        ///chassis.AddForce(gripImp, ForceMode.Impulse);

        // Vector3.Dot(angularVelocity, currentUp);
        // How much is our angularVelocity pointing in our up direction? i.e. Are we spinning?
        ///float spinSpeed = Vector3.Dot(angularVelocity, currentUp);
        //Debug.Log("Slide speed: " + slideSpeed);
        // Apply an angular impulse to compensate for spinning
        ///Vector3 antiSpinImp = currentUp * (-spinSpeed * mass * angularGrip);
        ///chassis.AddTorque(antiSpinImp, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Landscape")
        {
            onGround = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Landscape")
        {
            onGround = false;
        }
    }

    // Get use input
    void GetKeys()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
    }
}
