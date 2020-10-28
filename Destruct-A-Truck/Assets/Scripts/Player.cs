using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float forceImpulse;
    public float torqueImpulse;
    public float linearGrip;
    public float angularGrip;
    private float verticalInput;
    private float horizontalInput;
    public bool onGround;
    private Rigidbody truck;



    Player()
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
        truck = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GetKeys();
    }

    // Get use input
    void GetKeys()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
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
        Debug.Log("Current speed: " + currentSpeed);

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
    }
}
