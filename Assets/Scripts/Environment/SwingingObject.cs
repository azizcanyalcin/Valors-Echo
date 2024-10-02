using UnityEngine;

public class SwingingObject2D : MonoBehaviour
{
    [SerializeField] private float swingSpeed = 2f; // Speed of the swing
    [SerializeField] private float maxSwingAngle = 45f; // Maximum angle to swing

    private HingeJoint2D hingeJoint2d;
    private float currentAngle;

    void Start()
    {
        hingeJoint2d = GetComponent<HingeJoint2D>();
    }

    void Update()
    {
        // Calculate the target angle based on sine function for smooth swinging
        currentAngle = maxSwingAngle * Mathf.Sin(Time.time * swingSpeed);
        
        // Apply the target angle to the hinge joint
        hingeJoint2d.useMotor = true;
        JointMotor2D motor = hingeJoint2d.motor;
        motor.motorSpeed = swingSpeed * (currentAngle < 0 ? -1 : 1);
        motor.maxMotorTorque = 1000; // Adjust this value as needed
        hingeJoint2d.motor = motor;
    }
}
