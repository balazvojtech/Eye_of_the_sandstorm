using UnityEngine;

public class MachineDropper : MonoBehaviour
{
    public float dropDuration = 3f;
    public AudioClip fallingSound; // machine falling sound

    private bool isDropping = false;
    private Vector3 initialPosition;
    private Vector3 finalPosition;
    private AudioSource audioSource;

    private void Start()
    {
        // Save the initial position as the final position
        finalPosition = transform.position;

        // Move the machine up in the sky
        initialPosition = finalPosition + Vector3.up * 100f;
        transform.position = initialPosition;

        // Add an AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = fallingSound;
    }

    public void DropMachine()
    {
        // Start dropping the machine
        isDropping = true;

        // Play the falling sound
        audioSource.Play();
    }

    private void Update()
    {
        if (isDropping)
        {
            // Calculate the current progress of dropping
            float progress = Mathf.Clamp01(Time.deltaTime / dropDuration);

            // Move the machine towards its final position
            transform.position = Vector3.Lerp(transform.position, finalPosition, progress);

            // If the machine has reached its final position, stop dropping
            if (transform.position == finalPosition)
            {
                isDropping = false;
            }
        }
    }
}
