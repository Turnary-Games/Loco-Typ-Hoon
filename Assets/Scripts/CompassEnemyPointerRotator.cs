using UnityEngine;

public class CompassEnemyPointerRotator : MonoBehaviour
{
    public RectTransform rect;
    public Transform fromTransform;
    public Transform toTransform;
    public Camera relativeToCamera;

    [Range(0, 360)]
    public float angleOffset = 0;

    public void Reset()
    {
        rect = GetComponent<RectTransform>();
        relativeToCamera = Camera.main;
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

        var zAngle = Vector2.SignedAngle(Vector2.up, toVec - fromVec) + angleOffset;

        if (relativeToCamera)
        {
            zAngle += relativeToCamera.transform.eulerAngles.y;
        }

        var angle = rect.localEulerAngles;
        angle.z = zAngle;
        rect.localEulerAngles = angle;
    }

    static Vector2 Vec2XZ(Vector3 vec)
    {
        return new Vector2(vec.x, vec.z);
    }
}
