using UnityEngine;

public class ShepherdsCrook : MonoBehaviour
{
    [SerializeField] private float pushForce = 10f; // Adjustable in Inspector
    [SerializeField] private float maxDistance = 5f; // Max range of the crook
    [SerializeField] private LayerMask playerLayer; // Set this to detect only players

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            TryPushPlayer();
        }
    }

    void TryPushPlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, playerLayer))
        {
            Rigidbody targetRb = hit.collider.GetComponent<Rigidbody>();
            if (targetRb != null)
            {
                Vector3 pushDirection = (hit.transform.position - transform.position).normalized;
                targetRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            }
        }
    }
}
