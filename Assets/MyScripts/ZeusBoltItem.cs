using UnityEngine;

public class ZeusBoltItem : MonoBehaviour
{
    private Transform originalParent;
    private Rigidbody rb;
    private bool isHeld = false;

    public float throwForce = 10f; // Adjust for desired throw strength

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalParent = transform.parent;
    }

    void Update()
    {
        if (isHeld && Input.GetKeyDown(KeyCode.E))
        {
            ThrowSpear();
        }
    }

    public void AttachToHand(Transform hand)
    {
        isHeld = true;
        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        rb.isKinematic = true; // Stop physics while holding
    }

    void ThrowSpear()
    {
        isHeld = false;
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
    }
}
