using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    public Transform target;

    public void OnEnable()
    {
        if (!target)
        {
            Debug.LogWarning("Target transform must be set.", this);
            enabled = false;
        }
    }

    public void LateUpdate()
    {
        transform.position = target.position;
        transform.rotation = target.rotation;
    }
}
