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

        if (heldItem != null && Input.GetMouseButtonDown(0))
        {
            var crook = heldItem.GetComponent<ShepherdsCrook>();
            if (crook != null)
            {
                crook.Use();
            }
            else
            {
                var arcItem = heldItem.GetComponent<JugBHVR>();
                if (arcItem != null)
                {
                    arcItem.Throw(Camera.main.transform);
                    heldItem = null;
                }
                else
                {
                    var item = heldItem.GetComponent<ZeusBoltItem>();
                    if (item != null)
                    {
                        item.Throw(Camera.main.transform.forward);
                        heldItem = null;
                    }
                }
            }
        }

    }

    void TryPickupItem()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * pickupRange, Color.green, 1f);

        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange, interactableLayer))
        {
            Debug.Log($"Hit item: {hit.collider.name}");

            GameObject target = hit.collider.gameObject;

            if (target.TryGetComponent(out ZeusBoltItem item))
            {
                heldItem = target;
                item.Pickup(handTransform);
            }
            else if (target.TryGetComponent(out ShepherdsCrook crook))
            {
                heldItem = target;
                crook.Pickup(handTransform);
            }
            else if (target.TryGetComponent(out JugBHVR arcItem))
            {
                heldItem = target;
                arcItem.Pickup(handTransform);
            }
            else
            {
                Debug.Log("Hit something interactable-layered but it has no pickup logic.");
            }
        }
        else
        {
            Debug.Log("No interactable item hit.");
        }
    }


    void DropItem()
    {
        if (heldItem == null) return;

        if (heldItem.TryGetComponent(out ZeusBoltItem item))
        {
            item.Drop();
        }
        else if (heldItem.TryGetComponent(out ShepherdsCrook crook))
        {
            crook.Drop();
        }
        else if (heldItem.TryGetComponent(out JugBHVR arcItem))
        {
            arcItem.Drop();
        }

        Debug.Log($"Dropped: {heldItem.name}");
        heldItem = null;
    }

}
