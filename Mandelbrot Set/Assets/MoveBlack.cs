using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MoveBlack : MonoBehaviour
{

    private bool IsOver = false;
    [SerializeField] private Canvas canvas;
    public static Action HasMoved;
    public static Action HasStoped;

    private void Start()
    {
        if(gameObject.name != "BlackCirlce")
        {
            gameObject.GetComponent<Image>().color = Color.gray;
            enabled = false;
        }
     
    }
    private void LateUpdate()
    {
        if (IsOver)
        {
            HasMoved?.Invoke();
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
        IsOver = true;
    }
}
