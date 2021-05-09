using UnityEngine;
using UnityEngine.UI;

public class SwapImageFromPlayerShooting : MonoBehaviour
{
    public Button button;
    public PlayerShooting player;

    public Sprite spriteA;
    public Sprite spriteB;

    public void Reset()
    {
        button = GetComponent<Button>();
        if (button && button.image)
        {
            spriteA = button.image.sprite;
        }
    }

    public void RefreshFlippedFromPlayer()
    {
        button.image.sprite = player.cannonIsFlippedToTheLeft ? spriteA : spriteB;
    }
}
