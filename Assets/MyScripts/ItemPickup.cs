using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Transform hand; // The player's hand object
    private GameObject currentItem;

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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Ray from screen center

        // Draw the ray in the scene view for debugging (green for clear, red if it hits)
        Debug.DrawRay(ray.origin, ray.direction * 3f, Color.green, 0.5f);

        if (Physics.Raycast(ray, out hit, 3f))
        {
            Debug.Log($"Raycast hit: {hit.collider.name}", hit.collider.gameObject);
            Debug.DrawRay(ray.origin, ray.direction * 3f, Color.red, 0.5f); // Change to red when hitting something

            GameObject item = hit.collider.gameObject;
            if (item.CompareTag("Item"))
            {
                PickupItem(item);
            }
        }
    }


    void PickupItem(GameObject item)
    {
        if (currentItem != null) return; // Prevent picking up multiple items

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Disable physics so it stays in place
        }

        Transform attachPoint = item.transform.Find("AttachPoint");

        if (attachPoint != null)
        {
            Debug.Log($"AttachPoint found on {item.name}: {attachPoint.position}");

            item.transform.SetParent(hand);
            item.transform.localPosition = -attachPoint.localPosition; // Invert AttachPoint position
            item.transform.localRotation = Quaternion.Inverse(attachPoint.localRotation); // Invert rotation
        }
        else
        {
            Debug.LogWarning($"No AttachPoint found on {item.name}, attaching directly to hand!");
            item.transform.SetParent(hand);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
        }

        currentItem = item;
    }

    void DropItem()
    {
        if (currentItem == null) return; // No item to drop

        Debug.Log($"Dropping: {currentItem.name}");

        currentItem.transform.SetParent(null); // Unparent from hand

        Rigidbody rb = currentItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Enable physics again
            rb.AddForce(transform.forward * 2f, ForceMode.Impulse); // Optional: Small push forward
        }

        currentItem = null;
    }
}
