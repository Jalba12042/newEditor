using UnityEngine;
using System.Collections;

public class ReturnOnFloorTouch : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Rigidbody rb;

    public float returnDelay = 2f;
    public float returnSpeed = 5f;
    public string floorTag = "Ground"; // Set this tag on your floor objects

    private bool returning = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(floorTag) && !returning)
        {
            StartCoroutine(ReturnToOrigin());
        }
    }

    IEnumerator ReturnToOrigin()
    {
        returning = true;
        yield return new WaitForSeconds(returnDelay);

        rb.isKinematic = true;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * returnSpeed;
            transform.position = Vector3.Lerp(transform.position, originalPosition, t);
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, t);
            yield return null;
        }

        rb.isKinematic = false;
        returning = false;
    }
}
