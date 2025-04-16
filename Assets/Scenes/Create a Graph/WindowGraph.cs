using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;


public class WindowGraph : MonoBehaviour
{

    RectTransform graphContainer;
    [SerializeField] Sprite dotSprite;

    [SerializeField] RectTransform pf_TemplateX , pf_TemplateY;
    [SerializeField] private RectTransform dashX, dashY;

    private void Awake()
    {
        graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();
        pf_TemplateX = graphContainer.Find("pf_TemplateX").GetComponent<RectTransform>();
        pf_TemplateY = graphContainer.Find("pf_TemplateY").GetComponent<RectTransform>();
        dashX = graphContainer.Find("DashX").GetComponent<RectTransform>();
        dashY = graphContainer.Find("DashY").GetComponent<RectTransform>();

        List<int> valueList = new List<int>();
        for (int i = 0; i < 10; i++)
        {
            valueList.Add(UnityEngine.Random.Range(0, 100));
        }

        BarChartVisual barChartVisual = new BarChartVisual(graphContainer, 0.8f, Color.green);
        LineChartVisual lineChartVisual = new LineChartVisual(graphContainer, dotSprite, Color.blue, new Color(1, 1, 1, .5f));
        ShowGraph(lineChartVisual, valueList , -1 , (int i) => { return $"Day{i + 1}".ToString();} , (float i) => { return (i != -1f) ? Mathf.RoundToInt(i).ToString() : "" ; });
        
    }

    private void ShowGraph(IGraphVisual iGraphVisual , List<int> valueList ,int maxVisibleValueAmount = -1, Func<int , string> GetAxisLabelX = null , Func<float , string> GetAxisLabelY = null)
    {

        /*
        if(GetAxisLabelX == null)
        {
            GetAxisLabelX = (int i) => { return (i + 1).ToString(); };
        }

        if (GetAxisLabelY == null)
        {
            GetAxisLabelY = (float i) => { return (i).ToString(); };
        }
        */

        float height = graphContainer.GetComponent<RectTransform>().sizeDelta.y;
        float width = graphContainer.GetComponent<RectTransform>().sizeDelta.x;

        float maximumY = float.MinValue;
        float minimumY = float.MaxValue;

        if (maxVisibleValueAmount <= 0)
        {
            maxVisibleValueAmount = valueList.Count;
        }

        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount , 0); i < valueList.Count; i++)
        {
            if (valueList[i] > maximumY)
            {
                maximumY = valueList[i];
            }

            if (valueList[i] < minimumY)
            {
                minimumY = valueList[i];
            }
        }

        float yDifference = maximumY - minimumY;
        if (yDifference <= 0)
        {
            yDifference = 5f;
        }

        maximumY += (yDifference * 0.2f);
        minimumY -= (yDifference * 0.2f);

        float xSize = width / (maxVisibleValueAmount + 1);
        int xIndex = 0;
        

        
        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount , 0); i < valueList.Count; i++)
        {
            float xPos = xSize + xIndex * xSize;
            float yPos = (valueList[i] - minimumY) / (maximumY - minimumY) * height;
            
            iGraphVisual.AddGraphVisual(new Vector2(xPos, yPos), xSize, i, valueList);

            RectTransform goX = Instantiate(pf_TemplateX, graphContainer).GetComponent<RectTransform>();
            goX.anchoredPosition = new Vector2(xPos, 0);
            goX.GetComponent<Text>().text = GetAxisLabelX(i);

            RectTransform goDashX = Instantiate(dashX, graphContainer).GetComponent<RectTransform>();
            goDashX.anchoredPosition = new Vector2(xPos, 0);

            xIndex++;
        }

        int yCount = 10;
        for (int i = 0; i <= yCount; i++)
        {
            float normalizeValue = i * 1f / yCount;
            RectTransform goY = Instantiate(pf_TemplateY, graphContainer).GetComponent<RectTransform>();
            goY.anchoredPosition = new Vector2(0, normalizeValue * height);

            float output = minimumY + normalizeValue * (maximumY - minimumY);
            if (i == 0 && output < 0f)
            {
                output = 0f;
            }
            else if(output < 0f)
            {
                output = -1;
            }

            goY.GetComponent<Text>().text = GetAxisLabelY(output);

            RectTransform goDashY = Instantiate(dashY, graphContainer).GetComponent<RectTransform>();
            goDashY.anchoredPosition = new Vector2(0, normalizeValue * height);
        }


        
    }

    
    public interface IGraphVisual
    {
        public RectTransform AddGraphVisual(Vector2 graphPostion, float graphPositionWidth, int i, List<int> valueList);
    }

    public class BarChartVisual : IGraphVisual
    {

        RectTransform graphContainer;
        float multiplier;
        Color color;

        public BarChartVisual(RectTransform graphContainer, float multiplier, Color color )
        {
            this.graphContainer = graphContainer;
            this.multiplier = multiplier;
            this.color = color;
        }

        public RectTransform AddGraphVisual(Vector2 graphPostion, float graphPositionWidth , int i , List<int> valueList)
        {
            RectTransform rect = CreateBar(graphPostion, graphPositionWidth * multiplier , i , valueList);

            return rect;

        }

        private RectTransform CreateBar(Vector2 graphPostion, float graphPositionWidth, int i, List<int> valueList)
        {
            GameObject dot = new GameObject("Bar", typeof(Image), typeof(EventTrigger));
            dot.transform.SetParent(graphContainer, false);

            RectTransform rect = dot.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(graphPostion.x, 0);
            rect.sizeDelta = new Vector2(graphPositionWidth, graphPostion.y);
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.zero;
            rect.pivot = new Vector2(0.5f, 0f);

            rect.GetComponent<Image>().color = color;

            EventTrigger eventTrigger = rect.GetComponent<EventTrigger>();
            if (eventTrigger != null)
            {
                EventTrigger.Entry entry = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerEnter
                };
                entry.callback.AddListener((e) => { Debug.Log($"{i}  , {valueList[i]} "); });
                eventTrigger.triggers.Add(entry);
            }

            return rect;
        }


    }

    public class LineChartVisual : IGraphVisual
    {
        RectTransform graphContainer;
        Sprite dotSprite;
        Color dotColor;
        Color lineColor;
        RectTransform rect;

        public LineChartVisual(RectTransform graphContainer, Sprite dotSprite ,Color dotColor , Color lineColor)
        {
            this.graphContainer = graphContainer;
            this.dotSprite = dotSprite;
            this.dotColor = dotColor;
            this.lineColor = lineColor;

            rect = null;
        }
        public RectTransform AddGraphVisual(Vector2 graphPostion, float graphPositionWidth, int i, List<int> valueList)
        {
            RectTransform dotRect = CreateDot(graphPostion);

            if (rect != null)
            {
                CreateConnection(rect.anchoredPosition, dotRect.anchoredPosition);
            }

            rect = dotRect;
            return rect;
        }

        private RectTransform CreateDot(Vector2 anchoredPosition)
        {
            GameObject dot = new GameObject("Dot", typeof(Image), typeof(EventTrigger));
            dot.transform.SetParent(graphContainer, false);
            dot.GetComponent<Image>().sprite = dotSprite;
            dot.GetComponent<Image>().color = dotColor;

            RectTransform rect = dot.GetComponent<RectTransform>();
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = new Vector2(20f, 20f);
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.zero;

            return rect;
        }

        private void CreateConnection(Vector2 dotA, Vector2 dotB)
        {
            Vector2 dir = (dotB - dotA).normalized;
            float distance = Vector2.Distance(dotA, dotB);

            GameObject go = new GameObject("Line", typeof(Image));

            go.transform.SetParent(graphContainer, false);
            go.GetComponent<Image>().color = lineColor;
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.zero;
            rect.sizeDelta = new Vector2(distance, 5f);
            rect.pivot = new Vector2(0f, 0.5f);
            rect.anchoredPosition = dotA;
            rect.localEulerAngles = new Vector3(0f, 0f, Utils.GetAngleFromVector(dir));

        }
    }



}
