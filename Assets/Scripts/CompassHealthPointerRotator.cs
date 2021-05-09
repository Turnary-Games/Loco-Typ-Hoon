using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CompassHealthPointerRotator : MonoBehaviour
{
    public RectTransform rect;
    public Transform fromTransform;
    public Camera relativeToCamera;

    public string pickupTag;
    public bool filterOutDisabledPickups = true;
    public Image disabledWhenNoneFound;

    [Range(0, 360)]
    public float angleOffset = 0;

    [Header("Populated automatically on start")]
    public List<HealPlayer> pickups = new List<HealPlayer>();

    public void Reset()
    {
        rect = GetComponent<RectTransform>();
        relativeToCamera = Camera.main;
        disabledWhenNoneFound = GetComponentInChildren<Image>();
    }

    public void Start()
    {
        pickups.AddRange(GameObject.FindGameObjectsWithTag(pickupTag).Select(o => o.GetComponent<HealPlayer>()).Except(pickups));
    }

    public void OnEnable()
    {
        if (!fromTransform)
        {
            Debug.LogWarning($"The '{nameof(fromTransform)}' field must be set.", this);
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
        var toVec = FindNearestPickupPosXZ(fromVec);

        disabledWhenNoneFound.enabled = toVec.HasValue;
        if (toVec == null)
        {
            return;
        }

        var zAngle = Vector2.SignedAngle(Vector2.up, toVec.Value - fromVec) + angleOffset;

        if (relativeToCamera)
        {
            zAngle += relativeToCamera.transform.eulerAngles.y;
        }

        var angle = rect.localEulerAngles;
        angle.z = zAngle;
        rect.localEulerAngles = angle;
    }

    Vector2? FindNearestPickupPosXZ(Vector2 fromVec)
    {
        Vector2? nearestPos = null;
        float nearestDistance = float.PositiveInfinity;

        foreach (var pickup in pickups)
        {
            if (filterOutDisabledPickups && !pickup.enabled)
            {
                continue;
            }

            Vector2 toVec = Vec2XZ(pickup.transform.position);
            var dist = Vector2.Distance(fromVec, toVec);

            if (dist < nearestDistance)
            {
                nearestPos = toVec;
                nearestDistance = dist;
            }
        }

        return nearestPos;
    }

    static Vector2 Vec2XZ(Vector3 vec)
    {
        return new Vector2(vec.x, vec.z);
    }
}
