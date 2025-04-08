using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;

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

    public void Throw(Vector3 direction)
    {
        Drop();
        rb.AddForce(direction * 40f, ForceMode.VelocityChange); // Adjust throw force as needed
    }
}
