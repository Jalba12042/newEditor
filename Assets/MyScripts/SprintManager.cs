using UnityEngine;
using UnityEngine.UI;

public class SprintManager : MonoBehaviour
{
    public float sprintDuration = 3f;      // How long sprint lasts
    public float sprintCooldown = 5f;      // Cooldown after sprint
    public Image sprintUIImage;            // Your glowing button image
    public Outline glowEffect;             // Reference to the Outline component

    private bool isSprinting = false;
    private bool canSprint = true;
    private float sprintTimer = 0f;
    private float cooldownTimer = 0f;

    void Update()
    {
        HandleSprintInput();
        UpdateSprintTimers();
    }

    void HandleSprintInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canSprint)
        {
            StartSprint();
        }
    }

    void StartSprint()
    {
        isSprinting = true;
        canSprint = false;
        sprintTimer = sprintDuration;

        // Turn off glow while sprinting
        SetGlow(false);

        // TODO: Call your character's speed increase here
        Debug.Log("Sprint started!");
    }

    void UpdateSprintTimers()
    {
        if (isSprinting)
        {
            sprintTimer -= Time.deltaTime;
            if (sprintTimer <= 0f)
            {
                isSprinting = false;
                cooldownTimer = sprintCooldown;

                // TODO: Reset speed here
                Debug.Log("Sprint ended. Cooldown started.");
            }
        }
        else if (!canSprint)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                canSprint = true;
                SetGlow(true);
                Debug.Log("Sprint ready again!");
            }
        }
    }

    void SetGlow(bool on)
    {
        if (glowEffect != null)
            glowEffect.enabled = on;
    }
}
