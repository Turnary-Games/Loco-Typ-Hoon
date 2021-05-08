using UnityEngine;

public class CompassRotator : MonoBehaviour
{
    public RectTransform rect;
    public Transform fromTransform;
    public Transform toTransform;

    [Range(0, 360)]
    public float angleOffset = 90;

    public void Reset()
    {
        rect = GetComponent<RectTransform>();
    }

    public void OnEnable()
    {
        if (!fromTransform)
        {
            Debug.LogWarning($"The '{nameof(fromTransform)}' field must be set.", this);
            enabled = false;
        }
        else if (!toTransform)
        {
            Debug.LogWarning($"The '{nameof(toTransform)}' field must be set.", this);
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
        var fromVec = Vec2XZ(fromTransform.position);
        var toVec = Vec2XZ(toTransform.position);

        var zAngle = Vector2.SignedAngle(Vector2.up, fromVec - toVec) + angleOffset;
        var angle = rect.localEulerAngles;
        angle.z = zAngle;
        rect.localEulerAngles = angle;
    }

    static Vector2 Vec2XZ(Vector3 vec)
    {
        return new Vector2(vec.x, vec.z);
    }
}
