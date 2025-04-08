using UnityEngine;

public class JugBHVR : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;

    public float throwForce = 10f;
    public float arcAngle = 45f;

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

    public void Throw(Transform cameraTransform)
    {
        Drop();

        // Calculate the throw direction with an upward arc
        Vector3 forward = cameraTransform.forward;
        Vector3 upward = Vector3.up;

        // Rotate the forward vector upward by the arc angle
        Vector3 throwDirection = Quaternion.AngleAxis(arcAngle, cameraTransform.right) * forward;
        throwDirection.Normalize();

        rb.AddForce(throwDirection * throwForce, ForceMode.VelocityChange);

        Debug.Log($"Jug thrown with arc. Direction: {throwDirection}");
    }
}
