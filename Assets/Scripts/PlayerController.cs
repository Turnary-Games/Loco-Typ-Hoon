using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody locomotive;
    public Rigidbody[] carts;

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

    void Reset()
    {
        locomotive = GetComponentInChildren<Rigidbody>();
    }

    void OnEnable()
    {
        if (!locomotive)
        {
            Debug.LogWarning("Rigidbody must be assigned.", this);
            enabled = false;
        }
    }

    void Update()
    {
        inputForward = Mathf.Clamp(inputForward + Input.GetAxis("Vertical") * Time.deltaTime, 0, 1);
        inputTurn = Mathf.Clamp(inputTurn + Input.GetAxis("Horizontal") * Time.deltaTime, -1, 1);
    }

    void FixedUpdate()
    {
        ApplyForwardVelocity(locomotive, inputTurn * inputForward * maxTurnDegrees);
        foreach (var car in carts)
        {
            ApplyForwardVelocity(car);
        }
    }

    void ApplyForwardVelocity(Rigidbody body, float additionalYAngle = 0f)
    {
        var rotation = Quaternion.Euler(0, body.transform.eulerAngles.y + additionalYAngle, 0);
        body.velocity = rotation * Vector3.forward * inputForward * maxSpeedForward;
        body.rotation = rotation;
    }
}
