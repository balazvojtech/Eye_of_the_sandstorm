using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Reference to the target (capsule) GameObject
    public float rotationSpeed = 2f; // Speed of camera rotation
    public float maxVerticalAngle = 80f; // Maximum vertical angle the camera can rotate

    private float verticalRotation = 0f;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target not assigned to Camera Controller!");
            enabled = false; // Disable the script if target is not assigned
            return;
        }
    }

    void LateUpdate()
    {
        // Set camera's position to match the capsule's
        transform.position = target.position;

        // Camera rotation with mouse input
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        // Rotate the camera vertically within the specified angle limits
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxVerticalAngle, maxVerticalAngle);
        transform.localEulerAngles = new Vector3(verticalRotation, transform.localEulerAngles.y + mouseX, 0f);
    }
}
