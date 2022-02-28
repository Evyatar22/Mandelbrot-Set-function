using System;
using CodeMonkey.Utils;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AutoAxis : MonoBehaviour
{
    #region Data

    [SerializeField] private TMP_Text axisText;
    [SerializeField] private Canvas CanvasParent;
    [SerializeField] private RectTransform XTampPrfab;

    [SerializeField] private MoveRed RedCircle;
    [SerializeField] private MoveBlack BlackCircle;

    public int itarations = 50;
    public int AmountOfString = 10;
    public float widthOfAxis = 2.5f;
    [SerializeField] private bool hasparty = true;
    [SerializeField] private bool HasWire = false;

    public bool animated = false;
    public float deltaTime;
    private bool moved = false;
    private readonly List<Circle> AllCircles = new List<Circle>();
    private readonly List<Wire> AllWires = new List<Wire>();

    private RectTransform canvasRectTransform;

    #endregion

    #region setup

    private enum Axis
    {
        X,
        Y,
    }

    private void Awake()
    {
        canvasRectTransform = CanvasParent.GetComponent<RectTransform>();

        SetupCircles();
        CrateAxis(Axis.X);
        CrateAxis(Axis.Y);
    }

    private void CrateAxis(Axis axis)
    {
        var sizeDelta = canvasRectTransform.sizeDelta;
        
        var xwid = sizeDelta.x;
        var ywid = sizeDelta.y;

        switch (axis)
        {
            case Axis.X:
                var xAxis = new GameObject("XAxis");
                xAxis.transform.SetParent(CanvasParent.transform);

                var xAxisTr = xAxis.AddComponent<RectTransform>();
                xAxisTr.anchoredPosition = new Vector2(0, 0);
                xAxisTr.sizeDelta = new Vector2(0.01f, ywid);
                xAxis.AddComponent<Image>().color = Color.black;

                CrateText(Axis.X);
                break;

            case Axis.Y:
                var yAxis = new GameObject("YAxis");
                yAxis.transform.SetParent(CanvasParent.transform);
                var yAxisTr = yAxis.AddComponent<RectTransform>();

                var rotation = Quaternion.Euler(0f, 0f, 90f);
                yAxisTr.rotation = rotation;
                yAxisTr.anchoredPosition = new Vector2(0, 0);
                yAxisTr.sizeDelta = new Vector2(0.01f, xwid);
                yAxis.AddComponent<Image>().color = Color.black;

                CrateText(Axis.Y);
                break;
        }
    }

    //crate text for x Axis, y
    private void CrateText(Axis axis)
    {
        switch (axis)
        {
            case Axis.X:
                for (var i = 0; i < AmountOfString; i++)
                {
                    var textIns = Instantiate(axisText, Vector3.zero, Quaternion.identity);
                    textIns.rectTransform.SetParent(CanvasParent.transform);
                    textIns.rectTransform.anchoredPosition = new Vector2(-0.03f, (i * 0.2f));
                    textIns.text = textIns.rectTransform.anchoredPosition.y.ToString();

                    if (0.2 * -i != 0)
                    {
                        var textInsMinus = Instantiate(axisText, Vector3.zero, Quaternion.identity);
                        textInsMinus.rectTransform.SetParent(CanvasParent.transform);
                        textInsMinus.rectTransform.anchoredPosition = new Vector2(-0.03f, (i * -0.2f));
                        textInsMinus.text = "-" + textIns.rectTransform.anchoredPosition.y.ToString();
                    }
                }

                break;
            case Axis.Y:
                for (var i = 0; i < AmountOfString; i++)
                {
                    var textIns = Instantiate(axisText, Vector3.zero, Quaternion.identity);
                    textIns.rectTransform.SetParent(XTampPrfab);
                    textIns.rectTransform.anchoredPosition = new Vector2(-0.03f, (i * 0.2f));
                    textIns.text = textIns.rectTransform.anchoredPosition.y.ToString();

                    if (0.2 * -i != 0)
                    {
                        var textInsMinus = Instantiate(axisText, Vector3.zero, Quaternion.identity);
                        textInsMinus.rectTransform.SetParent(XTampPrfab);
                        textInsMinus.rectTransform.anchoredPosition = new Vector2(-0.03f, (i * -0.2f));
                        textInsMinus.text = "-" + textIns.rectTransform.anchoredPosition.y.ToString();
                    }
                }

                var spin = Quaternion.Euler(0, 0, -90f);
                XTampPrfab.rotation = spin;
                break;
        }
    }

    #endregion

    #region Actions

    public void AddItration()
    {
        itarations += 1;
    }

    public void Party()
    {
        hasparty = true;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        RedCircle.Animator.enabled = animated;
        BlackCircle.Animator.enabled = animated;
    }

    private void LateUpdate()
    {
        moved = RedCircle.IsDragging || BlackCircle.IsDragging || animated;

        if (moved)
        {
            UpdateCircles();
        }

        if (hasparty)
        {
            foreach (var game in AllCircles)
            {
                game.Image.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            }

            foreach (var game in AllWires)
            {
                game.Image.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            }
        }
    }

    #endregion

    #region Updateing

    private void SetupCircles()
    {
        var x = BlackCircle.RectTransform.anchoredPosition.x;
        var y = BlackCircle.RectTransform.anchoredPosition.y;
        AllCircles.Add(BlackCircle);

        for (var i = 1; i < itarations; i++)
        {
            var redCircleVec2 = RedCircle.RectTransform.anchoredPosition;
            var lastCircleVec2 = AllCircles[i - 1].RectTransform.anchoredPosition;
            var circle = Instantiate(BlackCircle, Vector3.zero, Quaternion.identity);

            circle.transform.SetParent(CanvasParent.transform);
            AllCircles.Add(circle);

            x = Mathf.Clamp(Mathf.Pow(x, 2), -Screen.width, Screen.width);
            x = Mathf.Clamp(x - Mathf.Pow(lastCircleVec2.y, 2), -Screen.width, Screen.width);
            x = Mathf.Clamp(x + redCircleVec2.x, -Screen.width, Screen.width);

            y = Mathf.Clamp((2 * lastCircleVec2.x * lastCircleVec2.y) + redCircleVec2.y, -Screen.width, Screen.width);

            circle.RectTransform.anchoredPosition = new Vector2(x, y);

            if (HasWire)
            {
                var wire = GenerateWire(lastCircleVec2, circle.RectTransform.anchoredPosition);

                AllWires.Add(wire);
            }
        }
    }

    private void UpdateCircles()
    {
        var x = BlackCircle.RectTransform.anchoredPosition.x;

        for (var i = 1; i < itarations; i++)
        {
            var redCircleVec2 = RedCircle.RectTransform.anchoredPosition;
            var lastCircleVec2 = AllCircles[i - 1].RectTransform.anchoredPosition;
            var circle = AllCircles[i].RectTransform;

            x = Mathf.Clamp(Mathf.Pow(x, 2), -Screen.width, Screen.width);
            x = Mathf.Clamp(x - Mathf.Pow(lastCircleVec2.y, 2), -Screen.width, Screen.width);
            x = Mathf.Clamp(x + redCircleVec2.x, -Screen.width, Screen.width);

            var y = Mathf.Clamp((2 * lastCircleVec2.x * lastCircleVec2.y) + redCircleVec2.y, -Screen.width, Screen.width);

            circle.anchoredPosition = new Vector2(x, y);
            if (HasWire)
            {
                UpdateWire(lastCircleVec2, circle.anchoredPosition, i);
            }
        }
    }

    private Wire GenerateWire(Vector2 nodeA, Vector2 nodeB)
    {
        var connector = new GameObject("connector").AddComponent<Wire>();
        connector.transform.SetParent(CanvasParent.transform);
        connector.Image.color = new Color(0f, 0f, 0f, 0.25f);

        var connectorRT = connector.RectTransform;
        connectorRT.anchorMin = new Vector2(0, 0);
        connectorRT.anchorMax = new Vector2(0, 0);

        SetWireTransforms(nodeA, nodeB, connector);

        return connector;
    }

    private void UpdateWire(Vector2 nodeA, Vector2 nodeB, int i)
    {
        SetWireTransforms(nodeA, nodeB, AllWires[i - 1]);
    }

    private static void SetWireTransforms(Vector2 nodeA, Vector2 nodeB, Wire wire)
    {
        var dir = (nodeB - nodeA).normalized;
        var distance = Vector2.Distance(nodeA, nodeB);
        var connectorRT = wire.RectTransform;
        connectorRT.sizeDelta = new Vector2(distance, 0.005f);
        connectorRT.position = nodeA + dir * distance * .5f;
        connectorRT.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    }

    #endregion
}
