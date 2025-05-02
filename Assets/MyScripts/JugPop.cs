using UnityEngine;

public class JugPop : MonoBehaviour
{
    public string groundTag = "Floor";         // Tag your ground with this
    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public float upwardModifier = 0.5f;
    public LayerMask affectedLayers;           // Define what objects should be affected
    public GameObject explosionEffect;         // Optional particle effect prefab

    private bool hasExploded = false;

    void OnCollisionEnter(Collision collision)
    {
        if (!hasExploded && collision.collider.CompareTag(groundTag))
        {
            hasExploded = true;
            Explode();
        }
    }

    void Explode()
    {

        // Find all nearby objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, affectedLayers);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.attachedRigidbody;
            if (rb != null && rb != GetComponent<Rigidbody>())
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardModifier, ForceMode.Impulse);
            }
        }
        hasExploded = false;
    }
}
