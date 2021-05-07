using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    public float seconds = 5f;

    void Start()
    {
        Destroy(gameObject, seconds);
    }
}
