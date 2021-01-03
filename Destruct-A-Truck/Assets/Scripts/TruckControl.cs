using System.Collections;
using System.Collections.Generic;
//using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class TruckControl : MonoBehaviour
{
    public static int Score = 0;
    // Start is called before the first frame update

    private float verticalInput;
    private float horizontalInput;
    public Rigidbody truck;
    public float forceImpulse;
    public float torqueImpulse;
    public bool onGround;
    public float linearGrip;
    public float angularGrip;

    public float wheelBase;
    public float axelWidth;
    public float suspensionHeight;
    public float wheelRadius;
    public float suspensionStrength;
    public float suspensionDamping;

    TruckControl()
    {
        //initialize
        forceImpulse = 0.0f;
        torqueImpulse = 0.0f;
        linearGrip = 0.2f;
        angularGrip = 0.2f;
        wheelBase = 1.0f;
        axelWidth = 8.86f;
        suspensionHeight = 1f;
        wheelRadius = 2f;
        suspensionStrength = 40.0f;
        suspensionDamping = 2.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        verticalInput = 0.0f;
        horizontalInput = 0.0f;
        
    }

    // Update is called once per frame
    void Update()
    {
        GetKeys();

        void OnGUI()
        {
            GUI.Box(new Rect(50, 50, 200, 50), "Score is: " + Score);
        }

    }
    void FixedUpdate()
    {
        UpdateController();
        Suspend();
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

        Vector3 currentVelocity = truck.velocity;
        // How much is our current velocity pointing in our current direction?
        float currentSpeed = Vector3.Dot(currentVelocity, currentForward);
        //Debug.Log("Current speed: " + currentSpeed);

        Vector3 angularVelocity = truck.angularVelocity;
        float mass = truck.mass;


        if (Mathf.Abs(verticalInput) > 0.0f)
        {
            // Calculate the impulse
            Vector3 vimpulse = transform.forward * forceImpulse * verticalInput;
            // Draw the impulse 
            Utils.DrawArrow(transform.position, vimpulse, Color.blue);
            // Apply the impulse
            truck.AddForce(vimpulse, ForceMode.Impulse);
        }

        if (Mathf.Abs(horizontalInput) > 0.0f)
        {
            // Calculate the impulse
            Vector3 torque = transform.up * torqueImpulse * horizontalInput;
            // Draw the torque 
            Utils.DrawArrow(transform.position, torque, Color.red);
            // Apply the impulse
            truck.AddTorque(torque, ForceMode.Impulse);
        }


        // Work out how fast we're sliding left/right
        // An compensate according to the grip property
         Vector3.Dot(currentVelocity, currentRight);
        // How much is our current velocity pointing in our right direction? i.e. Are we sliding?
        float slideSpeed = Vector3.Dot(currentVelocity, currentRight);
        //Debug.Log("Slide speed: " + slideSpeed);
        // Apply an impulse to compensate for sliding
        Vector3 gripImp = currentRight * (-slideSpeed * mass * linearGrip);
        truck.AddForce(gripImp, ForceMode.Impulse);

        Vector3.Dot(angularVelocity, currentUp);
        // How much is our angularVelocity pointing in our up direction? i.e. Are we spinning?
        float spinSpeed = Vector3.Dot(angularVelocity, currentUp);
        Debug.Log("Slide speed: " + slideSpeed);
        // Apply an angular impulse to compensate for spinning
        Vector3 antiSpinImp = currentUp * (-spinSpeed * mass * angularGrip);
        truck.AddTorque(antiSpinImp, ForceMode.Impulse);
    }

    void Suspend()
    {
        Vector3 wheelPos = default;
        float totalSuspensionDist = suspensionHeight + wheelRadius;
        // Get the chassis down direction in world space
        Vector3 wd = transform.up;
           
            // Get the wheel position in world space
            Vector3 wheelPosWs = transform.TransformPoint(wheelPos);

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
                if (hit.distance < totalSuspensionDist)
                {
                    // Total Force = SuspensionForce - DampingForce 
                    Vector3 diff = hit.point - wheelPosWs;

                    // Calculate the suspension force
                    float suspensionForce = suspensionStrength *
                        (totalSuspensionDist - diff.magnitude);

                    // Calculate the damping force. The higher the velocity of the wheelPos point
                    // on the body the higher the damping force
                    Vector3 pointVelocity = truck.GetPointVelocity(wheelPosWs);
                    // How much is the point velocity pointing in the direction of the diff vector 
                    float dampingScale = Vector3.Dot(pointVelocity, diff);
                    float dampingForce = suspensionDamping * dampingScale;

                    // Apply the force in the direction of the diff vector and scale by mass
                    Vector3 force = (suspensionForce + dampingForce) *
                    -diff.normalized * truck.mass;
                    //Vector3 force = suspensionForce * -diff.normalized * chassis.mass;
                    truck.AddForceAtPosition(force, wheelPosWs);
                }
                // Update the wheel position based on the hit distance
                _ = wheelPosWs +
                        (-transform.up * (hit.distance - wheelRadius));
            }
            else
            {
                // Update the wheel position

            }

        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "plane")
        {
            onGround = true;
           
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "plane")
        {
            onGround = false;
            truck.angularVelocity = Vector3.zero;
            truck.velocity = Vector3.zero;
        }
    }

    // Get use input
    void GetKeys()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
    }


}
