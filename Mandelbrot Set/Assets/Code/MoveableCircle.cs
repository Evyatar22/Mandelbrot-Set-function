using System;
using UnityEngine;

public abstract class MoveableCircle : Circle
{
    [SerializeField]
    private Animator animator;

    public bool IsDragging { get; protected set; }

    public Animator Animator
    {
        get
        {
            if (animator) return animator;

            animator = GetComponent<Animator>();

            return animator;
        }
    }

    protected RectTransform canvasRectTransform;

    protected virtual void Awake()
    {
        canvasRectTransform = Canvas.transform as RectTransform;
    }

    private void OnMouseDown()
    {
        if (!enabled) return;

        if (animator) animator.enabled = false;
        IsDragging = true;
    }

    protected virtual void OnMouseUp()
    {
        if (!enabled) return;

        if (animator) animator.enabled = true;
        IsDragging = false;
    }

    protected virtual void OnMouseDrag()
    {
        if (!enabled) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Input.mousePosition, Canvas.worldCamera, out var pos);
        transform.position = Canvas.transform.TransformPoint(pos);
    }
}
