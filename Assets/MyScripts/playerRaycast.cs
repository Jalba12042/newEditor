using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float rayDistance = 10f;
    public LayerMask hitLayers;
    public Color debugRayColor = Color.red;

    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click to shoot
        {
            ShootRay();
        }
    }

    void ShootRay()
    {
        if (playerCamera == null) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, hitLayers))
        {
            Debug.Log("Hit: " + hit.collider.name);
        }

        Debug.DrawRay(ray.origin, ray.direction * rayDistance, debugRayColor);
    }
}
