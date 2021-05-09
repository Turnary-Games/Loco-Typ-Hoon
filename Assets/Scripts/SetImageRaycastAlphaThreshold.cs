using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetImageRaycastAlphaThreshold : MonoBehaviour
{
    public Image image;
    [Range(0, 1)]
    public float threshold;

    public void Reset()
    {
        image = GetComponent<Image>();
    }

    public void OnValidate()
    {
        image.alphaHitTestMinimumThreshold = threshold;
    }

    public void Start()
    {
        image.alphaHitTestMinimumThreshold = threshold;
    }
}
