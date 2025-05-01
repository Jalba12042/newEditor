using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public float pickupRange = 3f;
    public LayerMask interactableLayer;
    public Transform handTransform;

    private GameObject heldItem;
    private EquipmentUIController uiController;

    void Start()
    {
        uiController = Object.FindFirstObjectByType<EquipmentUIController>();
        if (uiController == null)
            Debug.LogError("EquipmentUIController not found in scene!");
    }

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
            if (heldItem.TryGetComponent(out ShepherdsCrook crook))
            {
                crook.Use();
            }
            else if (heldItem.TryGetComponent(out JugBHVR arcItem))
            {
                arcItem.Throw(Camera.main.transform);
                heldItem = null;
                uiController?.SetEquipmentDisplay(EquipmentUIController.ItemType.None);
            }
            else if (heldItem.TryGetComponent(out ZeusBoltItem bolt))
            {
                bolt.Throw(Camera.main.transform.forward);
                heldItem = null;
                uiController?.SetEquipmentDisplay(EquipmentUIController.ItemType.None);
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

            if (target.TryGetComponent(out ZeusBoltItem bolt))
            {
                heldItem = target;
                bolt.Pickup(handTransform);
                uiController?.SetEquipmentDisplay(EquipmentUIController.ItemType.Bolt);
                Debug.Log("Picked up BOLT");
            }
            else if (target.TryGetComponent(out ShepherdsCrook crook))
            {
                heldItem = target;
                crook.Pickup(handTransform);
                uiController?.SetEquipmentDisplay(EquipmentUIController.ItemType.Crook);
                Debug.Log("Picked up CROOK");
            }
            else if (target.TryGetComponent(out JugBHVR jug))
            {
                heldItem = target;
                jug.Pickup(handTransform);
                uiController?.SetEquipmentDisplay(EquipmentUIController.ItemType.Jug);
                Debug.Log("Picked up JUG");
            }
            else
            {
                Debug.Log("Interactable hit, but no known item script.");
            }
        }
        else
        {
            Debug.Log("No interactable object hit.");
        }
    }

    void DropItem()
    {
        if (heldItem == null) return;

        if (heldItem.TryGetComponent(out ZeusBoltItem bolt))
        {
            bolt.Drop();
        }
        else if (heldItem.TryGetComponent(out ShepherdsCrook crook))
        {
            crook.Drop();
        }
        else if (heldItem.TryGetComponent(out JugBHVR jug))
        {
            jug.Drop();
        }

        Debug.Log($"Dropped: {heldItem.name}");
        heldItem = null;
        uiController?.SetEquipmentDisplay(EquipmentUIController.ItemType.None);
    }
}
