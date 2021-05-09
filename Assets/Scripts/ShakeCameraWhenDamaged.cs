using UnityEngine;

public class ShakeCameraWhenDamaged : MonoBehaviour, HealthScript.IOnDamagedEvent
{
    public float durationSeconds = 1f;
    public float intensity = 1f;

    public AudioSource[] playSounds = new AudioSource[0];

    public void OnDamaged(HealthScript.DamagedEvent data)
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

        foreach (var audioSource in playSounds)
        {
            audioSource.Play();
        }
    }
}
