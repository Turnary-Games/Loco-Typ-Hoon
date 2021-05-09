using UnityEngine;

public class ShakeCameraWhenContact : MonoBehaviour
{
    public float durationSeconds = 1f;
    public float intensity = 1f;
    public bool onCollision = true;
    public bool onTrigger = false;

    public void OnCollisionEnter(Collision _)
    {
        if (onCollision)
        {
            ShakeCamera();
        }
    }

    public void OnTriggerEnter(Collider _)
    {
        if (onTrigger)
        {
            ShakeCamera();
        }
    }

    void ShakeCamera()
    {
        var cam = Camera.main;
        if (!cam)
        {
            Debug.LogWarning("No main camera.", this);
            return;
        }

        var camShake = cam.GetComponent<CameraShake>();
        if (!camShake)
        {
            Debug.LogWarning($"Main camera does not have a '{nameof(CameraShake)}' script.", this);
        }

        camShake.StartShaking(durationSeconds, intensity);
    }
}
