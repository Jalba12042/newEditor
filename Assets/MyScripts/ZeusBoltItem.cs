using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider), typeof(AudioSource))]
public class ZeusBoltItem : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;
    private AudioSource audioSource;

    public AudioClip lightningLoopSound;
    public AudioClip[] throwOrDropSounds; // Set of sounds to randomly pick from

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();

        audioSource.loop = false;
        audioSource.playOnAwake = false;
    }

    public void Pickup(Transform hand)
    {
        rb.isKinematic = true;
        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        if (lightningLoopSound != null)
        {
            audioSource.clip = lightningLoopSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void Drop()
    {
        transform.SetParent(null);
        rb.isKinematic = false;

        StopLoopingSound();
        PlayRandomThrowOrDropSound();
    }

    public void Throw(Vector3 direction)
    {
        Drop(); // Unparent and stop loop

        rb.AddForce(direction * 40f, ForceMode.VelocityChange);
    }

    private void StopLoopingSound()
    {
        if (audioSource.isPlaying && audioSource.clip == lightningLoopSound)
        {
            audioSource.Stop();
            audioSource.loop = false;
        }
    }

    private void PlayRandomThrowOrDropSound()
    {
        if (throwOrDropSounds != null && throwOrDropSounds.Length > 0)
        {
            AudioClip randomClip = throwOrDropSounds[Random.Range(0, throwOrDropSounds.Length)];
            audioSource.PlayOneShot(randomClip);
        }
    }
}
