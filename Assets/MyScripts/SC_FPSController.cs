using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Needed for UI Outline

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
    private AudioSource sprintAudioSource;   // for sprint sound
    public AudioClip footstepClip;
    public AudioClip jumpClip;
    public AudioClip landClip;
    public AudioClip sprintClip;             // Sprint whoosh or dash sound

    // Sprint logic
    public float sprintDuration = 6f;
    public float sprintCooldown = 5f;
    private float sprintTimer = 0f;
    private float cooldownTimer = 0f;
    private bool canSprint = true;
    private bool isSprinting = false;

    // UI glow reference
    public Outline sprintGlowUI;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Setup movement audio
        movementAudioSource = GetComponent<AudioSource>();
        movementAudioSource.loop = true;
        movementAudioSource.playOnAwake = false;

        // Setup jump audio
        jumpAudioSource = gameObject.AddComponent<AudioSource>();
        jumpAudioSource.playOnAwake = false;
        jumpAudioSource.loop = false;

        // Setup sprint audio
        sprintAudioSource = gameObject.AddComponent<AudioSource>();
        sprintAudioSource.playOnAwake = false;
        sprintAudioSource.loop = false; // Set to true if you're using a looped sprint sound

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        wasGrounded = characterController.isGrounded;

        // Enable glow at start if sprint is ready
        if (sprintGlowUI != null)
            sprintGlowUI.enabled = true;
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isTryingToSprint = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isSprinting ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isSprinting ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // === Sprint Logic ===
        if (Input.GetKeyDown(KeyCode.LeftShift) && canSprint && !isSprinting)
        {
            isSprinting = true;
            sprintTimer = sprintDuration;
            canSprint = false;

            if (sprintGlowUI != null)
                sprintGlowUI.enabled = false;

            if (sprintClip != null)
            {
                sprintAudioSource.clip = sprintClip;
                sprintAudioSource.Play(); // ✅ Start sprint sound
            }

            Debug.Log("Sprint started");
        }

        if (isSprinting)
        {
            sprintTimer -= Time.deltaTime;

            // Stop sprint if Shift released OR time runs out
            if (!Input.GetKey(KeyCode.LeftShift) || sprintTimer <= 0f)
            {
                isSprinting = false;
                cooldownTimer = sprintCooldown;

                if (sprintAudioSource.isPlaying)
                    sprintAudioSource.Stop(); // ✅ Stop sprint sound instantly

                Debug.Log("Sprint ended. Cooldown started.");
            }
        }

        // Handle cooldown
        if (!canSprint && !isSprinting)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                canSprint = true;

                if (sprintGlowUI != null)
                    sprintGlowUI.enabled = true;

                Debug.Log("Sprint ready again!");
            }
        }

        // === Footstep Sound ===
        bool isWalking = characterController.isGrounded && (Mathf.Abs(curSpeedX) > 0.1f || Mathf.Abs(curSpeedY) > 0.1f);
        bool isMovingButNotSprinting = isWalking && !isSprinting;

        if (isMovingButNotSprinting)
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

        // === Jump ===
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

        // === Gravity ===
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // === Landing Sound ===
        if (!wasGrounded && characterController.isGrounded)
        {
            if (landClip != null)
                jumpAudioSource.PlayOneShot(landClip);
        }

        wasGrounded = characterController.isGrounded;

        // === Movement ===
        characterController.Move(moveDirection * Time.deltaTime);

        // === Camera Rotation ===
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }
}
