using UnityEngine;
using System.Collections;

public class SnapBack : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Rigidbody rb;

    public float delay = 2f;
    public float snapSpeed = 5f;
    public float positionThreshold = 0.05f;   // How close is “close enough”
    public float rotationThreshold = 1f;      // Degrees difference

    private bool returning = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!returning && !IsAtInitialTransform())
        {
            StartCoroutine(SnapBackRoutine());
        }
    }

    bool IsAtInitialTransform()
    {
        bool positionClose = Vector3.Distance(transform.position, initialPosition) < positionThreshold;
        bool rotationClose = Quaternion.Angle(transform.rotation, initialRotation) < rotationThreshold;
        return positionClose && rotationClose;
    }

    IEnumerator SnapBackRoutine()
    {
        returning = true;
        yield return new WaitForSeconds(delay);

        rb.isKinematic = true;

        float t = 0f;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        while (t < 1f)
        {
            t += Time.deltaTime * snapSpeed;
            transform.position = Vector3.Lerp(startPos, initialPosition, t);
            transform.rotation = Quaternion.Slerp(startRot, initialRotation, t);
            yield return null;
        }

        transform.position = initialPosition;
        transform.rotation = initialRotation;

        rb.isKinematic = false;
        returning = false;
    }
}
