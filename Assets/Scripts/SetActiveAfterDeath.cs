using UnityEngine;

public class SetActiveAfterDeath : MonoBehaviour, HealthScript.IOnDamagedEvent
{
    public bool activate = true;

    public GameObject[] setActive = new GameObject[0];
    public Behaviour[] setEnabled = new Behaviour[0];
    public Collider[] colliders = new Collider[0];

    public void OnDamaged(HealthScript.DamagedEvent data)
    {
        SetEnabledState(activate);
    }

    void SetEnabledState(bool enabled)
    {
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
