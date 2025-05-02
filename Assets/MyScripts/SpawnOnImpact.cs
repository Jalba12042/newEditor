using UnityEngine;

public class SpawnOnImpact : MonoBehaviour
{
    public string groundTag = "Ground";
    public GameObject spawnEffectPrefab; // Assign your prefab (e.g. lightning, smoke)
    public bool destroyAfterSpawn = true;

    private bool hasSpawned = false;

    void OnCollisionEnter(Collision collision)
    {
        if (hasSpawned) return;

        if (collision.collider.CompareTag(groundTag))
        {
            hasSpawned = true;

            // Use the first contact point for accuracy
            Vector3 spawnPosition = collision.contacts[0].point;
            Instantiate(spawnEffectPrefab, spawnPosition, Quaternion.identity);

            if (destroyAfterSpawn)
            {
                Destroy(gameObject);
            }

            hasSpawned = false;
        }
    }
}
