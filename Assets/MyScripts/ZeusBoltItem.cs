using UnityEngine;

public class ZeusBoltItem : MonoBehaviour
{
    public GameObject slowZonePrefab; // The slow area effect
    public float slowDuration = 5f;

    [Header("Idle Sound While Held")]
    public AudioClip idleSound; // The sound that plays while this item is being held

    void OnCollisionEnter(Collision collision)
    {
        // Check if we hit the ground or a valid surface
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            // Spawn the slow zone at the impact point
            GameObject slowZone = Instantiate(slowZonePrefab, transform.position, Quaternion.identity);

            // Start the countdown to destroy it
            Destroy(slowZone, slowDuration);

            // Destroy the bolt itself
            Destroy(gameObject);
        }
    }
}

