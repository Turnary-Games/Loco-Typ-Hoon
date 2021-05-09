using UnityEngine;

public class RotateTowardsTarget : MonoBehaviour
{
    public Transform target;

    public float speed = 1.0f;

    public void OnEnable()
    {
        if (!target)
        {
            Debug.LogWarning($"The '{nameof(target)}' field must be set.", this);
            enabled = false;
            return;
        }
    }

    public void Update()
    {
        var q = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, speed * Time.deltaTime);
    }
}
