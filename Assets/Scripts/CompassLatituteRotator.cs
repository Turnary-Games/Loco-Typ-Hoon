using UnityEngine;

public class CompassLatituteRotator : MonoBehaviour
{
    public RectTransform rect;
    public Camera cam;
    [Range(0, 360)]
    public float angleOffset = 0;

    public void Reset()
    {
        rect = transform as RectTransform;
        cam = Camera.main;
    }

    public void OnEnable()
    {
        if (!cam)
        {
            Debug.LogWarning($"The '{nameof(cam)}' field must be set.", this);
            enabled = false;
        }
        else if (!rect)
        {
            Debug.LogWarning($"The '{nameof(rect)}' field must be set.", this);
            enabled = false;
        }
    }

    public void Update()
    {
        Vector3 localEulerAngles = rect.localEulerAngles;
        localEulerAngles.z = cam.transform.eulerAngles.y + angleOffset;
        rect.localEulerAngles = localEulerAngles;
    }
}
