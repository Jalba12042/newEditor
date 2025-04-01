using UnityEngine;

public class ZeusBoltItem : MonoBehaviour
{
    public float throwForce = 15f;
    public GameObject slowZonePrefab; // Assign the Slow Zone prefab in the Inspector
    public float slowZoneDuration = 5f;

    private Rigidbody rb;
    private bool isHeld = false;

    [Header("Idle Sound While Held")]
    public AudioClip idleSound;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Disable physics while held
    }

    void Update()
    {
        if (isHeld && Input.GetMouseButtonDown(0)) // Left-click to throw
        {
            Throw();
        }
    }

    public void PickUp(Transform hand)
    {
        isHeld = true;
        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        rb.isKinematic = true;
    }

    public void Drop()
    {
        isHeld = false;
        transform.SetParent(null);
        rb.isKinematic = false;
    }

    private void Throw()
    {
        isHeld = false;
        transform.SetParent(null);
        rb.isKinematic = false;

        Vector3 throwDirection = Camera.main.transform.forward;
        rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
        Debug.Log("Lightning Bolt thrown!");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isHeld && collision.gameObject.CompareTag("Ground")) // If thrown and hits ground
        {
            Debug.Log("Lightning Bolt landed!");

            // Spawn Slow Zone
            Instantiate(slowZonePrefab, transform.position, Quaternion.identity);

            // Destroy the bolt after impact
            Destroy(gameObject);
        }
    }
}
