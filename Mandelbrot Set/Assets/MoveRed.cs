using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MoveRed : MonoBehaviour
{

    private bool IsOver = false;
    [SerializeField] private Canvas canvas;
    public static Action HasMoved;
    public static Action HasStoped;

    private void LateUpdate()
    {
        if (IsOver)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle
                (canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
            transform.position = canvas.transform.TransformPoint(pos);
        }


    }


    private void OnMouseExit()
    {
        HasStoped?.Invoke();
        IsOver = false;
    }

    private void OnMouseDrag()
    {
        HasMoved?.Invoke();
        IsOver = true;
    }
}
