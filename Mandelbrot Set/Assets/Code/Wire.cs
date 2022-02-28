using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class Wire : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image image;

    public RectTransform RectTransform
    {
        get
        {
            if (rectTransform) return rectTransform;

            rectTransform = this.EnsureComponent<RectTransform>();

            return rectTransform;
        }
    }

    public Image Image
    {
        get
        {
            if (image) return image;

            image = this.EnsureComponent<Image>();

            return image;
        }
    }
}
