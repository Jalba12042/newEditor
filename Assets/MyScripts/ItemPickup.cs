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
            else if (heldItem.TryGetComponent(out ZeusBoltItem bolt))
            {
                bolt.Use();
            }
            else if (heldItem.TryGetComponent(out JugBHVR jug))
            {
                jug.Use();
            }
        }
    }

    void TryPickupItem()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * pickupRange, Color.green, 1f);

        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange, interactableLayer))
        {
            GameObject target = hit.collider.gameObject;

            // CROOK
            if (target.TryGetComponent(out ShepherdsCrook crook))
            {
                heldItem = target;
                crook.playerCamera = Camera.main.transform;
                crook.Pickup(handTransform);
                uiController?.SetEquipmentDisplay(EquipmentUIController.ItemType.Crook);
                Debug.Log("Picked up CROOK");
            }
            // BOLT
            else if (target.TryGetComponent(out ZeusBoltItem bolt))
            {
                heldItem = target;
                bolt.Pickup(handTransform);
                bolt.SetCamera(Camera.main.transform); // Optional, if bolt needs camera in Use
                uiController?.SetEquipmentDisplay(EquipmentUIController.ItemType.Bolt);
                Debug.Log("Picked up BOLT");
            }
            // JUG
            else if (target.TryGetComponent(out JugBHVR jug))
            {
                heldItem = target;
                jug.SetCamera(Camera.main.transform);
                jug.Pickup(handTransform);
                uiController?.SetEquipmentDisplay(EquipmentUIController.ItemType.Jug);
                Debug.Log("Picked up JUG");
            }

            // Disable outline when picked up
            if (heldItem.TryGetComponent(out ItemOutlineController outlineCtrl))
                outlineCtrl.SetHeldState(true);
        }
        else
        {
            Debug.Log("No interactable object hit.");
        }
    }

    void DropItem()
    {
        if (heldItem == null) return;

        if (heldItem.TryGetComponent(out ShepherdsCrook crook))
        {
            crook.Drop();
        }
        else if (heldItem.TryGetComponent(out ZeusBoltItem bolt))
        {
            bolt.Drop();
        }
        else if (heldItem.TryGetComponent(out JugBHVR jug))
        {
            jug.Drop();
        }

        // Re-enable outline when dropped
        if (heldItem.TryGetComponent(out ItemOutlineController outlineCtrl))
            outlineCtrl.SetHeldState(false);

        heldItem = null;
        uiController?.SetEquipmentDisplay(EquipmentUIController.ItemType.None);
    }
}
