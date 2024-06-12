using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // reference to capsule - player
    public float rotationSpeed = 2f; 
    public float maxVerticalAngle = 80f; // vertical angle at which you can't overpass

    private float verticalRotation = 0f;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target not assigned to Camera Controller!");
            enabled = false; // disably if target is not assigned
            return;
        }
    }

    void LateUpdate()
    {
        // set camera's position to match the capsule's
        transform.position = target.position;

        // camera rotation with mouse movement
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        // rotate the camera vertically within the specified angle limits
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxVerticalAngle, maxVerticalAngle);
        transform.localEulerAngles = new Vector3(verticalRotation, transform.localEulerAngles.y + mouseX, 0f);
    }
}
