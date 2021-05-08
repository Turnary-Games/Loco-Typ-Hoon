using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, PlayerCartHealth.IOnDamagedEvent
{
    [FormerlySerializedAs("locomotive")]
    public Rigidbody engine;
    public Rigidbody[] carts;

    [Header("Cart settings")]
    public float cartLocalHingeAnchorOffset = 2f;
    public float engineLocalHingeAnchorOffset = 2f;

    [Header("Movement settings")]
    [Range(0, 45)]
    public float maxTurnDegrees = 15;
    [Range(0, 50)]
    public float[] speedGears = { 0, 3, 10 };
    [FormerlySerializedAs("directionGears")]
    public float[] steeringAngleGears = { -1.0f, -0.75f, -0.5f, -0.25f, 0.0f, 0.25f, 0.5f, 0.75f, 1.0f };
    public float acceleration = 1;
    public float deacceleration = 5;
    public float steeringAngleChangeSpeed = 0.5f;
    [Range(0, 1)]
    public float speedMultiplierWhenDamaged = 0;
    public bool resetToGear0WhenDamaged = true;

    [Header("UI elements")]
    public Slider speedSlider;

    [Header("Current input")]
    public float currentSpeed = 0;
    public float currentSteeringAngle = 4;
    public int inputSpeedGear = 0;
    public int inputSteeringAngleGear = 4;

    public void ShiftSpeedGear(float gear)
    {
        inputSpeedGear = Mathf.Clamp(Mathf.RoundToInt(gear), 0, speedGears.Length);
    }

    public void ShiftDirectionGear(float gear)
    {
        inputSteeringAngleGear = Mathf.Clamp(Mathf.RoundToInt(gear), 0, steeringAngleGears.Length);
    }

    public void OnDamaged(PlayerCartHealth.DamagedEvent data)
    {
        currentSpeed *= speedMultiplierWhenDamaged;
        if (resetToGear0WhenDamaged)
        {
            inputSpeedGear = 0;
            if (speedSlider)
            {
                speedSlider.value = 0;
            }
            else
            {
                Debug.LogWarning("No speed slider UI element was set when resetting the player speed.", this);
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        var frontColor = new Color(0, 0, 1, 0.5f);
        var backColor = new Color(1, 0, 0, 0.5f);

        if (engine)
        {
            var engineBackAnchor = GetBackAnchor(engine, cartLocalHingeAnchorOffset);
            Gizmos.color = backColor;
            Gizmos.DrawLine(engineBackAnchor, engine.transform.position);
            Gizmos.DrawSphere(engineBackAnchor, 0.05f);
        }

        foreach (var cart in carts)
        {
            if (!cart)
            {
                continue;
            }

            var frontAnchor = GetFrontAnchor(cart, cartLocalHingeAnchorOffset);
            var backAnchor = GetBackAnchor(cart, cartLocalHingeAnchorOffset);
            Gizmos.color = frontColor;
            Gizmos.DrawLine(frontAnchor, cart.transform.position);
            Gizmos.DrawSphere(frontAnchor, 0.05f);
            Gizmos.color = backColor;
            Gizmos.DrawLine(backAnchor, cart.transform.position);
            Gizmos.DrawSphere(backAnchor, 0.05f);
        }
    }

    public void OnEnable()
    {
        if (!engine)
        {
            Debug.LogWarning("Rigidbody for engine must be assigned.", this);
            enabled = false;
            return;
        }

        for (var i = 0; i < carts.Length; i++)
        {
            if (!carts[i])
            {
                Debug.LogWarning($"Rigidbody for cart index={i} must be assigned.", this);
                enabled = false;
                return;
            }
        }

        if (speedGears.Length == 0)
        {
            Debug.LogWarning("Must have at least 1 speed gear.", this);
            enabled = false;
        }

        if (steeringAngleGears.Length == 0)
        {
            Debug.LogWarning("Must have at least 1 steering angle gear.", this);
            enabled = false;
        }
    }

    public void Update()
    {
        inputSpeedGear = Mathf.Clamp(inputSpeedGear, 0, speedGears.Length);
        var targetSpeed = speedGears[inputSpeedGear];
        if (targetSpeed > currentSpeed)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, deacceleration * Time.deltaTime);
        }
        inputSteeringAngleGear = Mathf.Clamp(inputSteeringAngleGear, 0, steeringAngleGears.Length);
        currentSteeringAngle = Mathf.MoveTowardsAngle(
            currentSteeringAngle,
            steeringAngleGears[inputSteeringAngleGear],
            steeringAngleChangeSpeed * Time.deltaTime);
    }

    public void FixedUpdate()
    {
        MoveEngine(engine);

        var backAnchor = GetBackAnchor(engine, engineLocalHingeAnchorOffset);
        foreach (var cart in carts)
        {
            MoveCart(cart, backAnchor, out backAnchor);
        }
    }

    void MoveEngine(Rigidbody body)
    {
        var rotation = Quaternion.Euler(0, body.transform.eulerAngles.y + currentSteeringAngle * Mathf.Clamp01(Mathf.Abs(currentSpeed)) * maxTurnDegrees, 0);
        body.position += rotation * Vector3.forward * currentSpeed * Time.fixedDeltaTime;
        body.rotation = rotation;
    }

    void MoveCart(Rigidbody cart, Vector3 targetFrontAnchor, out Vector3 newBackAnchor)
    {
        newBackAnchor = GetNewBackAnchor(cart, targetFrontAnchor, cartLocalHingeAnchorOffset);
        Debug.DrawRay(targetFrontAnchor, Vector3.up, Color.blue, Time.fixedDeltaTime);
        Debug.DrawRay(newBackAnchor, Vector3.down, Color.red, Time.fixedDeltaTime);
        var pos = AverageVector3(targetFrontAnchor, newBackAnchor);
        cart.position = pos;
        var lookRotation = targetFrontAnchor - pos;
        if (lookRotation != Vector3.zero)
        {
            cart.rotation = Quaternion.LookRotation(lookRotation);
        }
    }

    Vector3 GetNewBackAnchor(Rigidbody cart, Vector3 targetFrontAnchor, float anchorOffsets)
    {
        var plane = new Plane(inNormal: cart.transform.right, inPoint: cart.position);
        var pointOnPlane = plane.ClosestPointOnPlane(targetFrontAnchor);

        var distToPlane = plane.GetDistanceToPoint(targetFrontAnchor);
        var distanceBetweenAnchors = anchorOffsets + anchorOffsets;

        // calc hypothenus
        var distanceFromPoint = Mathf.Sqrt(distanceBetweenAnchors * distanceBetweenAnchors - distToPlane * distToPlane);

        if (float.IsNaN(distanceFromPoint))
        {
            var vectorToPointOnPlane = (pointOnPlane - targetFrontAnchor).normalized;
            return targetFrontAnchor + vectorToPointOnPlane * distanceBetweenAnchors;
        }
        else
        {
            return pointOnPlane - cart.transform.forward * distanceFromPoint;
        }
    }

    Vector3 GetFrontAnchor(Rigidbody cart, float offset)
    {
        return cart.transform.TransformPoint(new Vector3(0, 0, offset));
    }

    Vector3 GetBackAnchor(Rigidbody cart, float offset)
    {
        return cart.transform.TransformPoint(new Vector3(0, 0, -offset));
    }

    Vector3 AverageVector3(Vector3 a, Vector3 b)
    {
        return (a + b) * 0.5f;
    }
}
