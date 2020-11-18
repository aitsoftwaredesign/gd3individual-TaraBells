using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class VehicleController3 : MonoBehaviour
{
    private float verticalInput;
    private float horizontalInput;
    private Rigidbody chassis;
    private Vector3[] wheelPos; 
    private GameObject[] wheel;


    public float forceImpulse;
    public float torqueImpulse;
    public float linearGrip;
    public float angularGrip;
    public float wheelBase;
    public float axelWidth;
    public float suspensionHeight;
    public float wheelRadius;
    public float suspensionStrength;
    public float suspensionDamping;
    public GameObject wheelPrefab;

    VehicleController3()
    {
        forceImpulse = 0.0f;
        torqueImpulse = 0.0f;
        linearGrip = 0.2f;
        angularGrip = 0.05f;
        wheelBase = 1.0f;
        axelWidth = 1.0f;
        suspensionHeight = 0.5f;
        wheelRadius = 0.3f;
        suspensionStrength = 40.0f;
        suspensionDamping = 2.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        verticalInput = 0.0f;
        horizontalInput = 0.0f;
        chassis = gameObject.GetComponent<Rigidbody>();
        // Lower center of mass
        chassis.centerOfMass = new Vector3(0.0f, -2.0f, 0.0f);
        InitializeWheels();
    }

    // Update is called once per frame
    void Update()
    {
        GetKeys();
    }
    void FixedUpdate()
    {
        UpdateWheels();
        UpdateController();
    }

    void UpdateController()
    {
        // Get the vechicles directions in world space
        Vector3 currentForward = transform.forward;
        Utils.DrawArrow(transform.position, currentForward * 2.0f, Color.blue);
        Vector3 currentRight = transform.right;
        Utils.DrawArrow(transform.position, currentRight * 2.0f, Color.red);
        Vector3 currentUp = transform.up;
        Utils.DrawArrow(transform.position, currentUp * 2.0f, Color.green);

        Vector3 currentVelocity = chassis.velocity;
        // How much is our current velocity pointing in our current forward direction?
        float currentSpeed = Vector3.Dot(currentVelocity, currentForward);
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
        float slideSpeed = Vector3.Dot(currentVelocity, currentRight);
        //Debug.Log("Slide speed: " + slideSpeed);
        // Apply an impulse to compensate for sliding
        Vector3 gripImp = -currentRight * (slideSpeed * mass * linearGrip);
        chassis.AddForce(gripImp, ForceMode.Impulse);

        // How much is our angularVelocity pointing in our up direction? i.e. Are we spinning?
        // Vector3.Dot(angularVelocity, currentUp);
        float spinSpeed = Vector3.Dot(angularVelocity, currentUp);
        //Debug.Log("Spin speed: " + spinSpeed);
        // Apply an angular impulse to compensate for spinning
        Vector3 antiSpinImp = -currentUp * (spinSpeed * mass * angularGrip);
        chassis.AddTorque(antiSpinImp, ForceMode.Impulse);
    }

    void InitializeWheels()
    {
        wheelPos = new Vector3[4];
        wheel = new GameObject[4];
        // Calculate the position of each wheel in local space
        wheelPos[0] = (wheelBase * 0.5f * Vector3.forward) +
            (axelWidth * 0.5f * Vector3.right);
        wheelPos[1] = (wheelBase * 0.5f * Vector3.forward) +
            (axelWidth * 0.5f * -Vector3.right);
        wheelPos[2] = (wheelBase * 0.5f * -Vector3.forward) +
            (axelWidth * 0.5f * Vector3.right);
        wheelPos[3] = (wheelBase * 0.5f * -Vector3.forward) +
            (axelWidth * 0.5f * -Vector3.right);


        for (int i = 0; i < wheel.Length; i++)
        {
            Vector3 wheelPosWs = transform.TransformPoint(wheelPos[i]);
            wheel[i] = Instantiate(wheelPrefab,
                wheelPosWs + (-transform.up * suspensionHeight),
                Quaternion.identity);
        }
        
    }

    void UpdateWheels()
    {
        float totalSuspensionDist = suspensionHeight + wheelRadius;
        // Get the chassis down direction in world space
        Vector3 wd = transform.up;
        // Debug: Draw the position of each wheelPos
        for(int i = 0; i < wheelPos.Length; i++)
        {
            // Get the wheel position in world space
            Vector3 wheelPosWs = transform.TransformPoint(wheelPos[i]);

            // Draw the ray we are about to cast
            Debug.DrawRay(wheelPosWs, 
                -transform.up * totalSuspensionDist, 
                Color.magenta);

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(wheelPosWs, 
                -transform.up, 
                out hit, totalSuspensionDist))
            { 
                if(hit.distance < totalSuspensionDist)
                {   
                    // Total Force = SuspensionForce - DampingForce 
                    Vector3 diff = hit.point - wheelPosWs;

                    // Calculate the suspension force
                    float suspensionForce = suspensionStrength * 
                        (totalSuspensionDist - diff.magnitude);

                    // Calculate the damping force. The higher the velocity of the wheelPos point
                    // on the body the higher the damping force
                    Vector3 pointVelocity = chassis.GetPointVelocity(wheelPosWs);
                    // How much is the point velocity pointing in the direction of the diff vector 
                    float dampingScale = Vector3.Dot(pointVelocity, diff);
                    float dampingForce = suspensionDamping * dampingScale;

                    // Apply the force in the direction of the diff vector and scale by mass
                    Vector3 force = (suspensionForce + dampingForce) * 
                    -diff.normalized * chassis.mass;
                    //Vector3 force = suspensionForce * -diff.normalized * chassis.mass;
                    chassis.AddForceAtPosition(force, wheelPosWs);  
                }
                // Update the wheel position based on the hit distance
                wheel[i].transform.position = wheelPosWs +
                        (-transform.up * (hit.distance - wheelRadius));
            }
            else 
            {
                // Update the wheel position
                wheel[i].transform.position = wheelPosWs +
                        (-transform.up * suspensionHeight);
            }

        }
    }

    // Get use input
    void GetKeys()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
    }
}
