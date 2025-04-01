using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Transform hand; // The player's hand object
    private GameObject currentItem;

    private AudioSource idleAudioSource;

    void Start()
    {
        // Create a new audio source just for idle sounds
        idleAudioSource = gameObject.AddComponent<AudioSource>();
        idleAudioSource.loop = true;
        idleAudioSource.playOnAwake = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Press E to pick up an item
        {
            TryPickupItem();
        }

        if (Input.GetMouseButtonDown(1)) // Right Click to drop
        {
            DropItem();
        }
    }

    void TryPickupItem()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.green, 1f);

        if (Physics.Raycast(ray, out hit, 3f))
        {
            Debug.Log($"Raycast hit: {hit.collider.name}", hit.collider.gameObject);
            Debug.DrawRay(ray.origin, ray.direction * 5f, Color.red, 1f);

            GameObject item = hit.collider.gameObject;

            if (item.CompareTag("Item"))
            {
                PickupItem(item);
            }
        }
    }

    void PickupItem(GameObject item)
    {
        if (currentItem != null) return;

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        Transform attachPoint = item.transform.Find("AttachPoint");

        if (attachPoint != null)
        {
            item.transform.SetParent(hand);
            item.transform.localPosition = -attachPoint.localPosition;
            item.transform.localRotation = Quaternion.Inverse(attachPoint.localRotation);
        }
        else
        {
            item.transform.SetParent(hand);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
        }

        currentItem = item;

        // Check for ZeusBoltItem component
        ZeusBoltItem bolt = currentItem.GetComponent<ZeusBoltItem>();
        if (bolt != null && bolt.idleSound != null)
        {
            idleAudioSource.clip = bolt.idleSound;
            idleAudioSource.Play();
        }
    }

    void DropItem()
    {
        if (currentItem == null) return;

        Debug.Log($"Dropping: {currentItem.name}");

        currentItem.transform.SetParent(null);

        Rigidbody rb = currentItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(transform.forward * 2f, ForceMode.Impulse);
        }

        // Stop idle sound
        idleAudioSource.Stop();

        currentItem = null;
    }

    public GameObject GetCurrentItem()
    {
        return currentItem;
    }
}
