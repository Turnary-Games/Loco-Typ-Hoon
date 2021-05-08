using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [FormerlySerializedAs("locomotive")]
    public Rigidbody engine;
    public Rigidbody[] carts;

    [Header("Cart settings")]
    public float cartLocalHingeAnchorOffset = 2f;
    public float engineLocalHingeAnchorOffset = 2f;

    [Header("Input settings")]
    [Range(0, 100)]
    public float maxSpeedForward = 5;
    [Range(0, 45)]
    public float maxTurnDegrees = 15;

    [Header("Current input")]
    [Range(0, 1)]
    public float inputForward = 0;
    [Range(-1, 1)]
    public float inputTurn = 0;

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
    }

    public void Update()
    {
        inputForward = Mathf.Clamp(inputForward + Input.GetAxis("Vertical") * Time.deltaTime, 0, 1);
        inputTurn = Mathf.Clamp(inputTurn + Input.GetAxis("Horizontal") * Time.deltaTime, -1, 1);
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
        var rotation = Quaternion.Euler(0, body.transform.eulerAngles.y + inputTurn * inputForward * maxTurnDegrees, 0);
        body.position += rotation * Vector3.forward * inputForward * maxSpeedForward * Time.fixedDeltaTime;
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
