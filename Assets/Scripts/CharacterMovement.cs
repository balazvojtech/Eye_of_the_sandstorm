using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public float rotationSpeed = 2f; 
    public float jumpForce = 5f; 
    public Transform cameraTransform; 
    public float jumpCooldown = 1f; 
    public float sprintSpeed = 8f; 
    public float interactionDistance = 2.0f; 
    public float holdDistance = 2.0f; 

    private Rigidbody rb;
    private bool canJump = true; // bool if i can jump
    private float originalMoveSpeed; 
    private bool isSprinting; // sprinting state (sprinting or not)
    private GameObject pickedUpObject = null; // object player picked up
    private Transform originalParent; // parent of the picked-up object that will be released
    private Vector3 originalScale; // original scale of the picked up object
    private Vector3 desiredScale = new Vector3(1f, 1f, 1f); // scale when dropping object so that it doesnt change shape

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // so that bean won't fall - stand upwards
        rb.freezeRotation = true;

        // check the camera reference
        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform not assigned!");
            enabled = false; 
        }

        // hide cursor + lock it to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        originalMoveSpeed = moveSpeed; 
    }

    void FixedUpdate()
    {
        // movement controls
        float moveHorizontal = Input.GetAxisRaw("Horizontal"); // getting input without smoothing
        float moveVertical = Input.GetAxisRaw("Vertical"); // getting input without smoothing

        // going straight where the camera is looking
        Vector3 forward = cameraTransform.forward;
        forward.y = 0; // keep movement horizontal - can't go up and down
        Vector3 movement = (forward * moveVertical + cameraTransform.right * moveHorizontal).normalized;

        // start movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        if (isSprinting)
        {
            Sprint();
        }

        // update the position and rotation of the picked-up object
        if (pickedUpObject != null)
        {
            UpdatePickedUpObjectPositionAndRotation();
        }
    }

    void Update()
    {
        // hide cursor when player clicks
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // show cursor when pressed esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // rotating camera
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up, mouseX * rotationSpeed);

        // jumping - cooldown so that you can't spam jump and fly in the sky with it
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false; // Disable jumping
            Invoke("ResetJump", jumpCooldown); // Reset jumping after cooldown
        }

        // shift must be held for sprinting
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartSprint();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            StopSprint();
        }

        // "E" for picking up crates
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
        moveSpeed = sprintSpeed;
    }

    void StopSprint()
    {
        moveSpeed = originalMoveSpeed;
        isSprinting = false;
    }

    void ResetJump()
    {
        canJump = true; //enable jump after cooldown
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
                pickedUpObject.transform.SetParent(cameraTransform, true); // save world position
                pickedUpObject.GetComponent<Rigidbody>().isKinematic = true;
                originalScale = pickedUpObject.transform.localScale; // store original scale so that it wont change shape
            }
        }
    }

    void DropObject()
{
    pickedUpObject.GetComponent<Rigidbody>().isKinematic = false;
    pickedUpObject.transform.SetParent(originalParent, true); // save world position
    pickedUpObject.transform.localScale = desiredScale; // reset object's scale - no shape change
    pickedUpObject = null;
}

    void UpdatePickedUpObjectPositionAndRotation()
    {
        Vector3 holdPosition = cameraTransform.position + cameraTransform.forward * holdDistance;
        pickedUpObject.transform.position = holdPosition; // move original object to where it was released
    }
}
