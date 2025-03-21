using UnityEngine;

public class ShepherdsCrook : MonoBehaviour
{
    public float pushForce = 10f; // Adjustable force from the Inspector
    public float maxRange = 5f; // Max range for the push effect

    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main; // Assign the main camera as the player's camera
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click to push
        {
            TryPushPlayer();
        }
    }

    void TryPushPlayer()
    {
        if (playerCamera == null) return; // Safety check

        RaycastHit hit;
        Vector3 rayOrigin = playerCamera.transform.position; // Start the ray from the camera
        Vector3 rayDirection = playerCamera.transform.forward; // Shoot forward

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, maxRange))
        {
            if (hit.collider.CompareTag("Player")) // Ensure we're hitting another player
            {
                Rigidbody targetRb = hit.collider.GetComponent<Rigidbody>();
                if (targetRb != null)
                {
                    Debug.Log("RayHit");
                    Vector3 pushDirection = rayDirection.normalized; // Push in the camera's forward direction
                    targetRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
                }
            }
        }
    }
}
