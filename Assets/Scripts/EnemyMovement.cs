using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;

    public float standStillWhenPlayerIsCloserThan = 30f;

    public float speed = 5f;

    public void OnEnable()
    {
        if (!player)
        {
            Debug.LogWarning($"The '{nameof(player)}' field must be set.");
            enabled = false;
            return;
        }
    }

    public void Update()
    {
        var vecToPlayer = player.position - transform.position;
        vecToPlayer.y = 0;

        if (vecToPlayer.magnitude < standStillWhenPlayerIsCloserThan)
        {
            return;
        }

        transform.position += vecToPlayer.normalized * speed * Time.deltaTime;
    }
}
