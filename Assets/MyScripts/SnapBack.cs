using UnityEngine;
using System.Collections;

public class SnapBack : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Rigidbody rb;

    public float delay = 2f;
    public float snapSpeed = 5f;

    private bool returning = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Start snap-back routine when the object is moved
        if (!returning)
        {
            StartCoroutine(SnapBackRoutine());
        }
    }

    IEnumerator SnapBackRoutine()
    {
        returning = true;
        yield return new WaitForSeconds(delay);

        rb.isKinematic = true;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * snapSpeed;
            transform.position = Vector3.Lerp(transform.position, initialPosition, t);
            transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, t);
            yield return null;
        }

        rb.isKinematic = false;
        returning = false;
    }
}
