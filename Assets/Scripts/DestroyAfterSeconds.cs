using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    public float seconds = 5f;

    public void Start()
    {
        Destroy(gameObject, seconds);
    }
}
