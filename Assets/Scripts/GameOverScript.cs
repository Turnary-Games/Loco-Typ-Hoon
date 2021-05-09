using UnityEngine;

public class GameOverScript : MonoBehaviour
{

    private static GameOverScript instance;

    [Header("GameObjects")]
    public GameObject[] gameObjectsToActivate = new GameObject[0];
    public GameObject[] gameObjectsToDeactivate = new GameObject[0];
    public GameObject[] gameObjectsToActivateOnWin = new GameObject[0];
    public GameObject[] gameObjectsToActivateOnLose = new GameObject[0];

    [Header("Behaviours")]
    public Behaviour[] behavioursToEnable = new Behaviour[0];
    public Behaviour[] behavioursToDisable = new Behaviour[0];

    [Header("Colliders")]
    public Collider[] collidersToEnable = new Collider[0];
    public Collider[] collidersToDisable = new Collider[0];

    public void OnEnable()
    {
        instance = this;
    }

    public static void SetGameOver(bool isWin)
    {
        if (!instance)
        {
            Debug.LogWarning("No GameOverScript instance!");
            return;
        }

        instance.SetEnabledState(isWin);
    }

    void SetEnabledState(bool isWin)
    {
        static void SetGameObjectActive(GameObject[] gameObjects, bool active)
        {
            if (gameObjects != null)
            {
                foreach (var gameObject in gameObjects)
                {
                    if (gameObject)
                    {
                        gameObject.SetActive(active);
                    }
                }
            }
        }
        SetGameObjectActive(gameObjectsToActivate, true);
        SetGameObjectActive(gameObjectsToDeactivate, false);
        SetGameObjectActive(gameObjectsToActivateOnWin, isWin);
        SetGameObjectActive(gameObjectsToActivateOnLose, !isWin);

        if (behavioursToEnable != null)
        {
            foreach (var behaviour in behavioursToEnable)
            {
                if (behaviour)
                {
                    behaviour.enabled = true;
                }
            }
        }

        if (behavioursToDisable != null)
        {
            foreach (var behaviour in behavioursToDisable)
            {
                if (behaviour)
                {
                    behaviour.enabled = false;
                }
            }
        }

        if (collidersToEnable != null)
        {
            foreach (var collider in collidersToEnable)
            {
                if (collider)
                {
                    collider.enabled = true;
                }
            }
        }

        if (collidersToDisable != null)
        {
            foreach (var collider in collidersToDisable)
            {
                if (collider)
                {
                    collider.enabled = false;
                }
            }
        }
    }
}
