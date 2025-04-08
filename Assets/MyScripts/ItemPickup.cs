using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public float pickupRange = 3f;
    public LayerMask interactableLayer;
    public Transform handTransform;

    private GameObject heldItem;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldItem == null)
                TryPickupItem();
            else
                DropItem();
        }

        if (heldItem != null && Input.GetMouseButtonDown(0)) // Left click
        {
            ThrowItem();
        }
    }

    void TryPickupItem()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * pickupRange, Color.green, 1f);

        if (Physics.Raycast(ray, out hit, pickupRange, interactableLayer))
        {
            Debug.Log($"Hit item: {hit.collider.name}");

            InteractableItem item = hit.collider.GetComponent<InteractableItem>();
            if (item != null)
            {
                heldItem = hit.collider.gameObject;
                item.Pickup(handTransform);
            }
        }
        else
        {
            Debug.Log("No interactable item hit.");
        }
    }

    void DropItem()
    {
        if (heldItem != null)
        {
            heldItem.GetComponent<InteractableItem>().Drop();
            heldItem = null;
        }
    }

    void ThrowItem()
    {
        if (heldItem != null)
        {
            heldItem.GetComponent<InteractableItem>().Throw(transform.forward);
            heldItem = null;
        }
    }
}
