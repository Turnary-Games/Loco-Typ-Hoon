using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Vector3 axisMultiplier = new Vector3(1, 0.5f, 0.2f);

    public AnimationCurve shakeAmountOverTime = AnimationCurve.EaseInOut(0, 1, 1, 0);

    [Header("These fields are set by script")]
    public float currentShakeDurationSeconds = 0f;
    public float currentShakeRemainingSeconds = 0f;
    public float currentShakeIntensity = 1;

    public void StartShaking(float duration, float intensity)
    {
        currentShakeRemainingSeconds = duration;
        currentShakeDurationSeconds = duration;
        currentShakeIntensity = intensity;
    }

    public void Update()
    {
        if (currentShakeRemainingSeconds <= 0)
        {
            return;
        }

        var time = 1 - currentShakeRemainingSeconds / currentShakeDurationSeconds;
        transform.localPosition = new Vector3(
            Random.value * axisMultiplier.x,
            Random.value * axisMultiplier.y,
            Random.value * axisMultiplier.z
        ) * shakeAmountOverTime.Evaluate(time);

        currentShakeRemainingSeconds -= Time.deltaTime;
        if (currentShakeRemainingSeconds <= 0)
        {
            currentShakeDurationSeconds = 0;
            transform.localPosition = Vector3.zero;
            return;
        }
    }
}
