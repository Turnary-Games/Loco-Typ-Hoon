using UnityEngine;

public class DisableAfterContact : MonoBehaviour
{
    public bool onCollision = true;
    public bool onTrigger = false;
    public bool shouldReenableAfterDelay = false;
    public float reenableAfterSeconds = 30f;

    public GameObject[] setActive = new GameObject[0];
    public Behaviour[] setEnabled = new Behaviour[0];
    public Collider[] colliders = new Collider[0];
    public AudioSource[] playSounds = new AudioSource[0];

    [Header("These fields are set by script")]
    public float secondsUntilReenabling = 0f;

    public void Reset()
    {
        setActive = new[] { gameObject };
        colliders = GetComponentsInChildren<Collider>();
    }

    public void OnCollisionEnter(Collision _)
    {
        if (onCollision)
        {
            foreach (var audioSource in playSounds)
            {
                audioSource.Play();
            }
            SetEnabledState(false);
        }
    }

    public void OnTriggerEnter(Collider _)
    {
        if (onTrigger)
        {
            foreach (var audioSource in playSounds)
            {
                audioSource.Play();
            }
            SetEnabledState(false);
        }
    }

    public void Update()
    {
        if (!shouldReenableAfterDelay || secondsUntilReenabling <= 0f)
        {
            return;
        }

        secondsUntilReenabling -= Time.deltaTime;

        if (secondsUntilReenabling <= 0f)
        {
            SetEnabledState(true);
            secondsUntilReenabling = 0f;
        }
    }

    void SetEnabledState(bool enabled)
    {
        if (shouldReenableAfterDelay)
        {
            secondsUntilReenabling = reenableAfterSeconds;
        }

        if (setActive != null)
        {
            foreach (var gameObject in setActive)
            {
                if (gameObject)
                {
                    gameObject.SetActive(enabled);
                }
            }
        }

        if (setEnabled != null)
        {
            foreach (var behaviour in setEnabled)
            {
                if (behaviour)
                {
                    behaviour.enabled = enabled;
                }
            }
        }

        if (colliders != null)
        {
            foreach (var collider in colliders)
            {
                if (collider)
                {
                    collider.enabled = enabled;
                }
            }
        }
    }
}
