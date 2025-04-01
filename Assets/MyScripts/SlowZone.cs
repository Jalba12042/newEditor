using UnityEngine;

public class SlowZone : MonoBehaviour
{
    public float slowAmount = 0.5f; // 50% speed reduction
    public float duration = 5f;

    void Start()
    {
        Destroy(gameObject, duration); // Destroy the slow zone after its duration
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // If a player enters the zone
        {
            SC_FPSController movement = other.GetComponent<SC_FPSController>();
            if (movement != null)
            {
                movement.walkingSpeed *= slowAmount;
                Debug.Log("Player entered slow zone!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // If a player leaves the zone
        {
            SC_FPSController movement = other.GetComponent<SC_FPSController>();
            if (movement != null)
            {
                movement.walkingSpeed /= slowAmount; // Restore speed
                Debug.Log("Player left slow zone!");
            }
        }
    }
}
