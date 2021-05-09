using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public Transform moveTargetToMoveAround;
    public RotateTowardsTarget headRotate;
    public bool updateHeadRotateWhenMoving = true;

    public float closestDistanceToPlayer = 10f;
    public float minDistanceEachMove = 10f;
    [Range(0, 90)]
    public float randomAngleToPlayer = 12f;

    public float minSpeed = 0.1f;
    public float maxSpeed = 5;
    public AnimationCurve speedMulPerDistTravelled = new AnimationCurve(new Keyframe(0, 0, 0, 1), new Keyframe(0.5f, 1), new Keyframe(1, 0, -1, 0));

    public float nextMoveInSeconds = 60f;
    public float moveIntervalRandomMin = 50f;
    public float moveIntervalRandomMax = 120f;

    [Header("These fields are set by script")]
    public bool isMoving = false;
    public float fullDistanceToMove;
    public Vector3 targetPosition;
    public Transform previousLookTarget;

    public void Reset()
    {
        headRotate = GetComponentInChildren<RotateTowardsTarget>();
    }

    public void OnEnable()
    {
        if (!player)
        {
            Debug.LogWarning($"The '{nameof(player)}' field must be set.");
            enabled = false;
            return;
        }

        if (!moveTargetToMoveAround)
        {
            Debug.LogWarning($"The '{nameof(moveTargetToMoveAround)}' field must be set.");
            enabled = false;
            return;
        }

        if (!headRotate && updateHeadRotateWhenMoving)
        {
            Debug.LogWarning($"The '{nameof(moveTargetToMoveAround)}' field should be set since '{nameof(updateHeadRotateWhenMoving)}' is set to true.");
        }
    }

    public void OnDrawGizmos()
    {
        if (isMoving)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, targetPosition);
            Gizmos.DrawSphere(targetPosition, 0.1f);
        }
    }

    public void Update()
    {
        moveTargetToMoveAround.position = targetPosition;

        if (isMoving)
        {
            MoveToPosition();
        }
        else
        {
            WaitForNextMove();
        }
    }

    void WaitForNextMove()
    {
        nextMoveInSeconds -= Time.deltaTime;

        if (nextMoveInSeconds <= 0)
        {
            var nextPos = CalcNewPosition();

            if (nextPos.HasValue)
            {
                targetPosition = nextPos.Value;
                fullDistanceToMove = (targetPosition - transform.position).magnitude;
                if (headRotate && updateHeadRotateWhenMoving)
                {
                    previousLookTarget = headRotate.target;
                    headRotate.target = moveTargetToMoveAround;
                }
                isMoving = true;
            }

            nextMoveInSeconds = Random.Range(moveIntervalRandomMin, moveIntervalRandomMax);
        }
    }

    void MoveToPosition()
    {
        var distanceTravelled = fullDistanceToMove - (targetPosition - transform.position).magnitude;
        var t = 1 - distanceTravelled / fullDistanceToMove;
        var speed = Mathf.Lerp(minSpeed, maxSpeed, speedMulPerDistTravelled.Evaluate(t)) * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed);

        if (Mathf.Approximately(distanceTravelled, fullDistanceToMove))
        {
            isMoving = false;

            if (headRotate && updateHeadRotateWhenMoving)
            {
                headRotate.target = previousLookTarget;
                previousLookTarget = null;
            }
        }
    }

    Vector3? CalcNewPosition()
    {
        var vecTowardsPlayer = player.position - transform.position;
        var distanceToPlayer = vecTowardsPlayer.magnitude;

        if (distanceToPlayer < minDistanceEachMove || distanceToPlayer <= closestDistanceToPlayer)
        {
            return null;
        }

        var moveDistance = Mathf.Lerp(minDistanceEachMove, distanceToPlayer - closestDistanceToPlayer, Random.value);
        var moveAngle = Random.Range(-1f, 1f) * randomAngleToPlayer;

        var moveVector = Quaternion.Euler(0, moveAngle, 0) * vecTowardsPlayer.normalized * moveDistance;
        return transform.position + moveVector;
    }
}
