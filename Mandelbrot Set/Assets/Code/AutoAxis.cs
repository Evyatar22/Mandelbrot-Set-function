using CodeMonkey.Utils;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AutoAxis : MonoBehaviour
{
    #region Data
    [SerializeField] private TMP_Text axisText;
    [SerializeField] private Transform CanvasPerent;
    [SerializeField] private RectTransform XTampPrfab;

    [SerializeField] private RectTransform RedCircle;
    [SerializeField] private RectTransform BlackCircle;

    public int itarations = 50;
    public int AmountOfString = 10;
    public float widthOfAxis = 2.5f;
    [SerializeField] private bool hasparty = true;
    [SerializeField] private bool HasWire = false;

    public bool animated = false;
    public float deltaTime;
    private bool moved = false;
    private List<GameObject> AllCircles = new List<GameObject>();
    private List<GameObject> AllWires = new List<GameObject>();

    #endregion

    #region setup
    private void Start()
    {
        MoveBlack.HasMoved += HasMoved;
        MoveBlack.HasStoped += HasStoped;

        MoveRed.HasMoved += HasMoved;
        MoveRed.HasStoped += HasStoped;

        StartCircles();
        CrateAxis(true);
        CrateAxis(false);

    }
    private void CrateAxis(bool Y_True)
    {

        float xwid = CanvasPerent.gameObject.GetComponent<RectTransform>().sizeDelta.x;
        float ywid = CanvasPerent.gameObject.GetComponent<RectTransform>().sizeDelta.y;

        if (Y_True)
        {
            GameObject YAxis = new GameObject("YAxis", typeof(Image));
            YAxis.transform.SetParent(CanvasPerent);
            RectTransform YAxisTr = YAxis.GetComponent<RectTransform>();

            Quaternion rotaion = Quaternion.Euler(0f, 0f, 90f);
            YAxisTr.rotation = rotaion;
            YAxisTr.anchoredPosition = new Vector2(0, 0);
            YAxisTr.sizeDelta = new Vector2(0.01f, xwid);
            YAxis.GetComponent<Image>().color = Color.black;

            CrateText(true);
        }
        else
        {
            GameObject XAxis = new GameObject("XAxis", typeof(Image));
            XAxis.transform.SetParent(CanvasPerent);

            RectTransform XAxisTr = XAxis.GetComponent<RectTransform>();
            XAxisTr.anchoredPosition = new Vector2(0, 0);
            XAxisTr.sizeDelta = new Vector2(0.01f, ywid);
            XAxis.GetComponent<Image>().color = Color.black;

            CrateText(false);
        }
    }

    //crate text for x Axis, y
    private void CrateText(bool Y_True)
    {
        if (Y_True)
        {
            for (int i = 0; i < AmountOfString; i++)
            {
                GameObject TextIns = Instantiate(axisText.gameObject, Vector3.zero, Quaternion.identity);
                TextIns.GetComponent<RectTransform>().SetParent(XTampPrfab);
                TextIns.GetComponent<RectTransform>().anchoredPosition = new Vector2(-0.03f, (i * 0.2f));
                TextIns.GetComponent<TMP_Text>().text = TextIns.GetComponent<RectTransform>().anchoredPosition.y.ToString();

                if (0.2 * -i != 0)
                {
                    GameObject TextInsMinus = Instantiate(axisText.gameObject, Vector3.zero, Quaternion.identity);
                    TextInsMinus.GetComponent<RectTransform>().SetParent(XTampPrfab);
                    TextInsMinus.GetComponent<RectTransform>().anchoredPosition = new Vector2(-0.03f, (i * -0.2f));
                    TextInsMinus.GetComponent<TMP_Text>().text = "-" + TextIns.GetComponent<RectTransform>().anchoredPosition.y.ToString();

                }
            }

            Quaternion Spin = Quaternion.Euler(0, 0, -90f);
            XTampPrfab.rotation = Spin;

        }
        else
        {
            for (int i = 0; i < AmountOfString; i++)
            {
                GameObject TextIns = Instantiate(axisText.gameObject, Vector3.zero, Quaternion.identity);
                TextIns.GetComponent<RectTransform>().SetParent(CanvasPerent);
                TextIns.GetComponent<RectTransform>().anchoredPosition = new Vector2(-0.03f, (i * 0.2f));
                TextIns.GetComponent<TMP_Text>().text = TextIns.GetComponent<RectTransform>().anchoredPosition.y.ToString();

                if (0.2 * -i != 0)
                {
                    GameObject TextInsMinus = Instantiate(axisText.gameObject, Vector3.zero, Quaternion.identity);
                    TextInsMinus.GetComponent<RectTransform>().SetParent(CanvasPerent);
                    TextInsMinus.GetComponent<RectTransform>().anchoredPosition = new Vector2(-0.03f, (i * -0.2f));
                    TextInsMinus.GetComponent<TMP_Text>().text = "-" + TextIns.GetComponent<RectTransform>().anchoredPosition.y.ToString();
                }
            }
        }
    }
    #endregion

    #region Actions
    private void OnDestroy()
    {
        MoveBlack.HasMoved -= HasMoved;
        MoveBlack.HasStoped -= HasStoped;

        MoveRed.HasMoved -= HasMoved;
        MoveRed.HasStoped -= HasStoped;
    }
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
    private void LateUpdate()
    {

        if (animated) { moved = true; }
        if (moved) { updateCircles(); }
        if (hasparty)
        {
            foreach (GameObject game in AllCircles)
            {
                game.GetComponent<Image>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            }
            foreach (GameObject game in AllWires)
            {
                game.GetComponent<Image>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            }
        }
    }
    private void HasMoved()
    {
        moved = true;
    }

    private void HasStoped()
    {
        moved = false;
    }


    #endregion

    #region Updateing
    private void updateCircles()
    {
        StartUpdateCircles();
    }


    private void StartCircles()
    {
        float x = BlackCircle.anchoredPosition.x;
        float y = BlackCircle.anchoredPosition.y;
        AllCircles.Add(BlackCircle.gameObject);

        for (int i = 1; i < itarations; i++)
        {
            Vector2 RedCircleVec2 = RedCircle.anchoredPosition;
            Vector2 LastCircleVec2 = AllCircles[i - 1].GetComponent<RectTransform>().anchoredPosition;
            GameObject Circle = Instantiate(BlackCircle.gameObject, Vector3.zero, Quaternion.identity);

            Circle.transform.SetParent(CanvasPerent);
            AllCircles.Add(Circle);

            x = Mathf.Pow(x, 2);
            x -= Mathf.Pow(LastCircleVec2.y, 2);
            x += RedCircleVec2.x;

            y = (2 * LastCircleVec2.x
                * LastCircleVec2.y) + RedCircleVec2.y;

            Circle.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);

            if (HasWire)
            {
                 GameObject wire = GenrateWireStart(LastCircleVec2
               , Circle.GetComponent<RectTransform>().anchoredPosition);

                AllWires.Add(wire);
            }

        }

    }

    private void StartUpdateCircles()
    {
        float x = BlackCircle.anchoredPosition.x;
        float y = BlackCircle.anchoredPosition.y;
        for (int i = 1; i < itarations; i++)
        {
            Vector2 RedCircleVec2 = RedCircle.anchoredPosition;
            Vector2 LastCircleVec2 = AllCircles[i - 1].GetComponent<RectTransform>().anchoredPosition;
            RectTransform ICircle = AllCircles[i].GetComponent<RectTransform>();


            x = Mathf.Pow(x, 2);
            x -= Mathf.Pow(LastCircleVec2.y, 2);
            x += RedCircleVec2.x;

            y = (2 * LastCircleVec2.x
                * LastCircleVec2.y) + RedCircleVec2.y;

            ICircle.anchoredPosition = new Vector2(x, y);
            if (HasWire)
            {
                
                  GenrateWireUpdate(LastCircleVec2
                    ,ICircle.anchoredPosition, i);
                
                
            }
        }
    }

    public GameObject GenrateWireStart(Vector2 NodeA, Vector2 NodeB)
    {
        GameObject Connector = new GameObject("connector", typeof(Image));
        Connector.transform.SetParent(CanvasPerent);
        RectTransform ConnectorRT = Connector.GetComponent<RectTransform>();

        ConnectorRT.anchorMin = new Vector2(0, 0);
        ConnectorRT.anchorMax = new Vector2(0, 0);

        Connector.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.25f);

        Vector2 dir = (NodeB - NodeA).normalized;
        float distance = Vector2.Distance(NodeA, NodeB);

        ConnectorRT.sizeDelta = new Vector2(distance, 0.005f);

        ConnectorRT.position = NodeA + dir * distance * .5f;

        ConnectorRT.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));

        return Connector;
    }

    public void GenrateWireUpdate(Vector2 NodeA, Vector2 NodeB, int i)
    {
        RectTransform ConnectorRT = AllWires[i - 1].GetComponent<RectTransform>();
        Vector2 dir = (NodeB - NodeA).normalized;
        float distance = Vector2.Distance(NodeA, NodeB);
        ConnectorRT.sizeDelta = new Vector2(distance, 0.005f);
        ConnectorRT.position = NodeA + dir * distance * .5f;
        ConnectorRT.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    }
    #endregion

}
