using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    public Transform target;

    void OnEnable()
    {
        if (!target)
        {
            Debug.LogWarning("Target transform must be set.", this);
            enabled = false;
        }
    }

    void LateUpdate()
    {
        transform.position = target.position;
    }
}
