using UnityEngine;

public class ItemOutlineController : MonoBehaviour
{
    public float lookAtRange = 4f;
    private Camera playerCamera;
    private Outline outline;
    private bool isHeld = false;

    void Start()
    {
        playerCamera = Camera.main;
        outline = GetComponent<Outline>();
        if (outline != null)
            outline.enabled = false;
    }

    void Update()
    {
        if (isHeld || outline == null || playerCamera == null)
            return;

        Vector3 dirToPlayer = transform.position - playerCamera.transform.position;
        float distance = dirToPlayer.magnitude;

        if (distance <= lookAtRange)
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, lookAtRange))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    outline.enabled = true;
                    return;
                }
            }
        }

        outline.enabled = false;
    }

    public void SetHeldState(bool held)
    {
        isHeld = held;
        if (outline != null)
            outline.enabled = !held;
    }
}
