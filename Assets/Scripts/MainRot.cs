using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class MainRot : MonoBehaviour
{

    private Vignette vignette;
    PostProcessVolume post_process;
    public float intensity = 0.4f;
    private float originalIntensity;

    public float sensitivity = 2.0f; // Mouse sensitivity
    //public Transform playerBody; // Player body to rotate along with the camera (optional)

    public float speed = 20.0f; // Speed multiplier
    [SerializeField]private float actualspeed = 0f;

    public static MainRot instance;



    public float forwardSpeed = 20.0f; // Speed multiplier for forward movement
    public float rollSpeed = 50f;      // Speed of rolling
    public float yawSpeed = 20f;       // Speed of turning (yaw)
    public float maxRollAngle = 45f;   // Maximum roll angle (like an airplane)
    public float stabilizationSpeed = 2.0f; // Speed at which the spaceship returns to neutral position

    private float targetRollAngle = 0.0f; // Target roll angle when turning
    private float currentYaw = 0.0f;      // Current yaw angle

    private float targetRotationX = 0.0f;  // Target vertical rotation
    private float rotationX = 0.0f;         // Current vertical rotation
    public float smoothTime = 0.2f;         // Smooth time for damping
    private float rotationVelocity = 0.0f;  // Used by SmoothDamp for tracking velocity

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        post_process = GetComponent<PostProcessVolume>();

        post_process.profile.TryGetSettings(out vignette);

        originalIntensity = intensity;



    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {
            MoveForward();
            HandleInput();
            RotateSpaceship();
        }
        
    }

   

    private void MoveForward()
    {
        // For giving spaceship acceleration
        if (actualspeed < speed) actualspeed += 0.5f;
        transform.Translate(Vector3.forward * actualspeed * Time.deltaTime);
    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            targetRollAngle = maxRollAngle;        // Roll to the left
            currentYaw -= yawSpeed * Time.deltaTime; // Turn to the left
        }
        else if (Input.GetKey(KeyCode.D))
        {
            targetRollAngle = -maxRollAngle;       // Roll to the right
            currentYaw += yawSpeed * Time.deltaTime; // Turn to the right
        }
        else
        {
            // No input, stabilize the roll angle
            targetRollAngle = 0.0f;
        }
    }

    private void RotateSpaceship()
    {

        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;
        targetRotationX -= mouseY; // Adjust the target rotation based on input
        targetRotationX = Mathf.Clamp(targetRotationX, -45f, 45f); // Clamp the target to prevent over-rotation

        // Smoothly adjust the current rotation to the target rotation
        rotationX = Mathf.SmoothDamp(rotationX, targetRotationX, ref rotationVelocity, smoothTime);

        // Smoothly adjust the roll angle to give a more realistic airplane effect
        float currentRoll = Mathf.LerpAngle(transform.localEulerAngles.z, targetRollAngle, Time.deltaTime * stabilizationSpeed);

        // Apply roll and yaw to the spaceship's rotation
        Quaternion targetRotation = Quaternion.Euler(rotationX, currentYaw, currentRoll);
        transform.rotation = targetRotation;
    }

    // Helper function to normalize angle between -180 and 180 degrees
    float NormalizeAngle(float angle)
    {
        angle = angle % 360;
        if (angle > 180) angle -= 360;
        return angle;
    }

    public void TakeDamage(float amount)
    {
        
        StartCoroutine(DecreaseIntensityOverTime());
    }

    public float damageRate = 0.1f;
    public float maxIntensity = 0.4f;

    IEnumerator DecreaseIntensityOverTime()
    {

        intensity = 0.7f;
        vignette.intensity.Override(intensity);

        while (intensity > 0)
        {
            intensity -= damageRate * Time.deltaTime;
            intensity = Mathf.Clamp(intensity, 0f, maxIntensity);
            vignette.intensity.value = intensity;
            yield return null;
        }

        
    }


}
