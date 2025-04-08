using UnityEngine;

public class ShepherdsCrook : MonoBehaviour
{
    public float pushForce = 10f;
    public float pushRange = 2.5f;
    public LayerMask pushableLayer;

    private Rigidbody rb;
    private Transform playerCamera;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = Camera.main.transform;
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
