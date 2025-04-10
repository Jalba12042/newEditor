using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider), typeof(AudioSource))]
public class JugBHVR : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;
    private AudioSource audioSource;

    public float throwForce = 10f;
    public float arcAngle = 45f;

    public AudioClip pickupSound;
    public AudioClip movementSound;
    public AudioClip impactSound;

    private Transform handTransform;
    private Vector3 lastHandPosition;
    private bool isHeld = false;
    private bool hasPlayedMoveSound = false;

    private float movementThreshold = 0.1f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    void Update()
    {
        if (isHeld && handTransform != null && !hasPlayedMoveSound)
        {
            Vector3 velocity = (handTransform.position - lastHandPosition) / Time.deltaTime;
            bool isMoving = velocity.magnitude > movementThreshold;

            if (isMoving && movementSound != null)
            {
                audioSource.PlayOneShot(movementSound);
                hasPlayedMoveSound = true;
            }

            lastHandPosition = handTransform.position;
        }
    }
    public void Pickup(Transform hand)
    {
        rb.isKinematic = true;
        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        handTransform = hand;
        lastHandPosition = hand.position;
        isHeld = true;
        hasPlayedMoveSound = false;

        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
    }

    public void Drop()
    {
        transform.SetParent(null);
        rb.isKinematic = false;
        isHeld = false;
        handTransform = null;
    }

    public void Throw(Transform cameraTransform)
    {
        Drop();

        Vector3 forward = cameraTransform.forward;
        Vector3 throwDirection = Quaternion.AngleAxis(arcAngle, cameraTransform.right) * forward;
        throwDirection.Normalize();

        rb.AddForce(throwDirection * throwForce, ForceMode.VelocityChange);
        Debug.Log($"Jug thrown with arc. Direction: {throwDirection}");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isHeld && impactSound != null)
        {
            AudioSource.PlayClipAtPoint(impactSound, transform.position);
        }
    }
}