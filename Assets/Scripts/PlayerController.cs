using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody body;

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
        body = GetComponentInChildren<Rigidbody>();
    }

    void OnEnable()
    {
        if (!body)
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
        var rotation = Quaternion.Euler(0, transform.eulerAngles.y + inputTurn * maxTurnDegrees, 0);
        body.velocity = rotation * Vector3.forward * inputForward * maxSpeedForward;
        body.rotation = rotation;
    }
}
