using UnityEngine;

public class ZeusBoltItem : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;
    private Transform playerCamera;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip holdLoopClip;
    public AudioClip throwClip;
    public AudioClip impactClip;

    private bool isHeld = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        if (audioSource != null)
        {
            audioSource.loop = false;
            audioSource.playOnAwake = false;
        }
    }

    void Update()
    {
        if (isHeld && !audioSource.isPlaying && holdLoopClip != null)
        {
            audioSource.clip = holdLoopClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void Pickup(Transform hand)
    {
        rb.isKinematic = true;
        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        isHeld = true;

        // Start looping hold sound
        if (audioSource != null && holdLoopClip != null)
        {
            audioSource.clip = holdLoopClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void Drop()
    {
        transform.SetParent(null);
        rb.isKinematic = false;
        isHeld = false;

        // Stop looping sound
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void Throw(Vector3 direction)
    {
        Drop();

        // Play throw sound once
        if (audioSource != null && throwClip != null)
        {
            audioSource.loop = false;
            audioSource.PlayOneShot(throwClip);
        }

        rb.AddForce(direction * 40f, ForceMode.VelocityChange);
        Debug.Log("Zeus Bolt thrown!");
    }

    public void SetCamera(Transform cam)
    {
        playerCamera = cam;
    }

    public void Use()
    {
        if (playerCamera != null)
        {
            Throw(playerCamera.forward);
        }
        else
        {
            Debug.LogWarning("Player camera not assigned to ZeusBoltItem.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isHeld && impactClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(impactClip);
        }
    }
}
