using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class SC_FPSController : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public Transform teleLastSentTo;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    bool wasGrounded;

    [HideInInspector]
    public bool canMove = true;

    // Audio
    private AudioSource movementAudioSource; // for footsteps
    private AudioSource jumpAudioSource;     // for jump/land
    public AudioClip footstepClip;
    public AudioClip jumpClip;
    public AudioClip landClip;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Setup movement audio
        movementAudioSource = GetComponent<AudioSource>();
        movementAudioSource.loop = true;
        movementAudioSource.playOnAwake = false;

        // Setup separate audio for jump/land
        jumpAudioSource = gameObject.AddComponent<AudioSource>();
        jumpAudioSource.playOnAwake = false;
        jumpAudioSource.loop = false;

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        wasGrounded = characterController.isGrounded;
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Play walking sound if grounded and moving
        bool isWalking = characterController.isGrounded && (Mathf.Abs(curSpeedX) > 0.1f || Mathf.Abs(curSpeedY) > 0.1f);
        if (isWalking)
        {
            if (!movementAudioSource.isPlaying && footstepClip != null)
            {
                movementAudioSource.clip = footstepClip;
                movementAudioSource.Play();
            }
        }
        else
        {
            if (movementAudioSource.isPlaying)
            {
                movementAudioSource.Stop();
            }
        }

        // Jump
        if (Input.GetButtonDown("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;

            if (jumpClip != null)
                jumpAudioSource.PlayOneShot(jumpClip);
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Gravity
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Landing sound
        if (!wasGrounded && characterController.isGrounded)
        {
            if (landClip != null)
                jumpAudioSource.PlayOneShot(landClip);
        }

        wasGrounded = characterController.isGrounded;

        // Move character
        characterController.Move(moveDirection * Time.deltaTime);

        // Camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

}
