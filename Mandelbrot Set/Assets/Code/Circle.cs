using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public abstract class Circle : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private Canvas canvas;

    public Canvas Canvas
    {
        get
        {
            if (canvas) return canvas;

            canvas = GetComponentInParent<Canvas>();

            return canvas;
        }
    }

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