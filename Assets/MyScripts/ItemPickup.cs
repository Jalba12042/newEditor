using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Transform handTransform; // The transform where the item will be held
    private GameObject heldItem;
    private Rigidbody heldItemRb;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Pick up with "E"
        {
            TryPickupItem();
        }

        if (Input.GetMouseButtonDown(1)) // Drop with right-click
        {
            DropItem();
        }
    }

    void TryPickupItem()
    {
        if (heldItem != null) return; // Prevent picking up multiple items

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 8f))
        {
            if (hit.collider.CompareTag("Item"))
            {
                heldItem = hit.collider.gameObject;
                heldItemRb = heldItem.GetComponent<Rigidbody>();

                if (heldItemRb)
                {
                    heldItemRb.isKinematic = true; // Disable physics while holding
                }

                heldItem.transform.SetParent(handTransform);
                heldItem.transform.localPosition = Vector3.zero; // Adjust position in hand
                heldItem.transform.localRotation = Quaternion.identity;
            }
        }
    }

    void DropItem()
    {
        if (heldItem == null) return;

        heldItem.transform.SetParent(null);

        if (heldItemRb)
        {
            heldItemRb.isKinematic = false; // Reactivate physics
            heldItemRb.AddForce(Camera.main.transform.forward * 2f, ForceMode.Impulse); // Add a small throw force
        }

        heldItem = null;
        heldItemRb = null;
    }
}
