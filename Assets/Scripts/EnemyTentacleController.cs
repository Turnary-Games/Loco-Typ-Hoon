using UnityEngine;

public class EnemyTentacleController : MonoBehaviour, DamageDealer.IOnDamageDealt
{
    private static readonly int isUpAnimProp = Animator.StringToHash("IsUp");

    public Animator animator;

    public float disappearAfterRandomMin = 10f;
    public float disappearAfterRandomMax = 15f;

    public float destroySecondsAfterGoingDown = 5f;

    [Header("These fields are set by script")]
    public float delayUntilDisappearing = 0f;

    public void Start()
    {
        delayUntilDisappearing = Random.Range(disappearAfterRandomMin, disappearAfterRandomMax);
    }

    public void Reset()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (delayUntilDisappearing <= 0)
        {
            return;
        }

        delayUntilDisappearing -= Time.deltaTime;

        if (delayUntilDisappearing <= 0)
        {
            GoDownAndSelfDestroy();
        }
    }

    public void OnDamageDealt(DamageDealer.DamageDealtEvent data)
    {
        GoDownAndSelfDestroy();
    }

    void GoDownAndSelfDestroy()
    {
        animator.SetBool(isUpAnimProp, false);
        Destroy(gameObject, destroySecondsAfterGoingDown);
    }
}
