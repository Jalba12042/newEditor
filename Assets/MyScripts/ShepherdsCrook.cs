using UnityEngine;

public class ShepherdsCrook : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;

    public Transform playerCamera;
    public LayerMask pushableLayer;

    public float pushRange;
    public float pushForce;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void Pickup(Transform hand)
    {
        rb.isKinematic = true;
        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Drop()
    {
        transform.SetParent(null);
        rb.isKinematic = false;
    }

    public void Use()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pushRange, pushableLayer))
        {
            Rigidbody hitRb = hit.collider.attachedRigidbody;
            if (hitRb != null)
            {
                Vector3 pushDir = hit.point - transform.position;
                pushDir.y = 0f; // Optional: keep the push mostly horizontal
                pushDir.Normalize();
                hitRb.AddForce(pushDir * pushForce, ForceMode.Impulse);
            }
        }
    }
}
