using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of normal movement
    public float rotationSpeed = 2f; // Speed of character rotation
    public float jumpForce = 5f; // Force applied when jumping
    public Transform cameraTransform; // Reference to the camera's transform
    public float jumpCooldown = 1f; // Cooldown time between jumps
    public float sprintSpeed = 8f; // Speed when sprinting
    public float interactionDistance = 2.0f; // Distance to interact with objects
    public float holdDistance = 2.0f; // Distance to hold the object in front of the camera

    private Rigidbody rb;
    private bool canJump = true; // Flag to determine if the character can jump
    private float originalMoveSpeed; // Original move speed
    private bool isSprinting; // Flag to track sprinting state
    private GameObject pickedUpObject = null; // Reference to the currently picked up object
    private Transform originalParent; // Original parent of the picked up object
    private Vector3 originalScale; // Original scale of the picked up object
    private Vector3 desiredScale = new Vector3(1f, 1f, 1f); // Desired scale when dropping the object

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Lock rotation of the Rigidbody
        rb.freezeRotation = true;

        // Check if the cameraTransform reference is assigned
        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform not assigned!");
            enabled = false; // Disable the script if cameraTransform is not assigned
        }

        // Hide cursor and lock it to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        originalMoveSpeed = moveSpeed; // Store original move speed
    }

    void FixedUpdate()
    {
        // Movement controls
        float moveHorizontal = Input.GetAxisRaw("Horizontal"); // Use GetAxisRaw to get input without smoothing
        float moveVertical = Input.GetAxisRaw("Vertical"); // Use GetAxisRaw to get input without smoothing

        // Calculate movement direction based on the camera's forward direction
        Vector3 forward = cameraTransform.forward;
        forward.y = 0; // Keep the movement horizontal
        Vector3 movement = (forward * moveVertical + cameraTransform.right * moveHorizontal).normalized;

        // Apply movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        // Handle sprinting
        if (isSprinting)
        {
            Sprint();
        }

        // Update the position and rotation of the picked-up object
        if (pickedUpObject != null)
        {
            UpdatePickedUpObjectPositionAndRotation();
        }
    }

    void Update()
    {
        // Hide cursor when clicking
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Show cursor when hitting escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Character rotation with mouse input
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up, mouseX * rotationSpeed);

        // Jumping with cooldown
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false; // Disable jumping
            Invoke("ResetJump", jumpCooldown); // Reset jumping after cooldown
        }

        // Check for sprinting input
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartSprint();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            StopSprint();
        }

        // Check for block interaction input
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pickedUpObject == null)
            {
                TryPickUpObject();
            }
            else
            {
                DropObject();
            }
        }
    }

    void StartSprint()
    {
        moveSpeed = sprintSpeed;
        isSprinting = true;
    }

    void Sprint()
    {
        // Increase movement speed while sprinting
        moveSpeed = sprintSpeed;
    }

    void StopSprint()
    {
        moveSpeed = originalMoveSpeed;
        isSprinting = false;
    }

    void ResetJump()
    {
        canJump = true; // Enable jumping after cooldown
    }

    void TryPickUpObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, interactionDistance))
        {
            if (hit.collider.CompareTag("Block"))
            {
                pickedUpObject = hit.collider.gameObject;
                originalParent = pickedUpObject.transform.parent;
                pickedUpObject.transform.SetParent(cameraTransform, true); // Preserve world position
                pickedUpObject.GetComponent<Rigidbody>().isKinematic = true;
                originalScale = pickedUpObject.transform.localScale; // Store original scale
            }
        }
    }

    void DropObject()
{
    // Check if the object is below the terrain
    RaycastHit hit;
    if (Physics.Raycast(pickedUpObject.transform.position, Vector3.down, out hit))
    {
        if (hit.point.y < 0)
        {
            // If the object is below the terrain, move it to the terrain surface
            pickedUpObject.transform.position = hit.point + Vector3.up * pickedUpObject.transform.localScale.y * 0.5f;
        }
    }

    pickedUpObject.GetComponent<Rigidbody>().isKinematic = false;
    pickedUpObject.transform.SetParent(originalParent, true); // Preserve world position
    pickedUpObject.transform.localScale = desiredScale; // Set the object's scale to the desired scale
    pickedUpObject = null;
}

    void UpdatePickedUpObjectPositionAndRotation()
    {
        Vector3 holdPosition = cameraTransform.position + cameraTransform.forward * holdDistance;
        pickedUpObject.transform.position = holdPosition; // Move the object to the hold position
    }
}
