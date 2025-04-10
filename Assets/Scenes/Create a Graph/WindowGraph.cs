using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;


public class WindowGraph : MonoBehaviour
{

    RectTransform graphContainer;
    [SerializeField] Sprite circleSprite;

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


        ShowGraph(valueList , (int i) => { return $"Day{i + 1}".ToString();} , (float i) => { return (i > 0) ? Mathf.RoundToInt(i).ToString() : ""; });
        
    }


    private RectTransform CreateCircle(Vector2 anchoredPosition)
    {
        GameObject circle = new GameObject("Circle" , typeof(Image) , typeof(EventTrigger));
        circle.transform.SetParent(graphContainer, false);
        circle.GetComponent<Image>().sprite = circleSprite;

        RectTransform rect = circle.GetComponent<RectTransform>();
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = new Vector2(20f, 20f);
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.zero;

        

        return rect;
    }


    private void ShowGraph(List<int> valueList , Func<int , string> GetAxisLabelX = null , Func<float , string> GetAxisLabelY = null)
    {

        
        if(GetAxisLabelX == null)
        {
            GetAxisLabelX = (int i) => { return (i + 1).ToString(); };
        }

        if (GetAxisLabelY == null)
        {
            GetAxisLabelY = (float i) => { return (i).ToString(); };
        }
        


        float height = graphContainer.GetComponent<RectTransform>().sizeDelta.y;
        
        float xSize = 100f;


        float maximumY = float.MinValue;
        float minimumY = float.MaxValue;

        for(int i = 0;i < valueList.Count;i++)
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

        maximumY += ((maximumY - minimumY) * 0.2f);
        minimumY -= ((maximumY - minimumY) * 0.2f);


        RectTransform rect = null;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPos = xSize + i * xSize;
            float yPos = (valueList[i] - minimumY) / (maximumY - minimumY) * height;
            RectTransform circleRect = CreateCircle(new Vector2(xPos, yPos));


            if (rect != null)
            {
                CreateConnection(rect.anchoredPosition, circleRect.anchoredPosition);
            }

            rect = circleRect;


            RectTransform goX = Instantiate(pf_TemplateX, graphContainer).GetComponent<RectTransform>();
            goX.anchoredPosition = new Vector2(xPos, 0);
            goX.GetComponent<Text>().text = GetAxisLabelX(i) ;

            RectTransform goDashX = Instantiate(dashX, graphContainer).GetComponent<RectTransform>();
            goDashX.anchoredPosition = new Vector2(xPos, 0);

            EventTrigger eventTrigger = rect.GetComponent<EventTrigger>();
            if (eventTrigger != null)
            {
                int tempI = i;
                EventTrigger.Entry entry = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerEnter
                };
                entry.callback.AddListener((e) => OnPointerEnterHover(tempI + 1, valueList[tempI]));
                eventTrigger.triggers.Add(entry);

            }
        }

        // Debug.Log(minimumY + "   " + maximumY);

        int yCount = 10;
        for (int i = 0; i <= yCount; i++)
        {
            RectTransform goY = Instantiate(pf_TemplateY, graphContainer).GetComponent<RectTransform>();
            goY.anchoredPosition = new Vector2(0, (i * 1f / yCount) * height);
            float normalizeValue = i * 1f / yCount;
            goY.GetComponent<Text>().text = GetAxisLabelY(minimumY + normalizeValue * (maximumY - minimumY)) ;

            RectTransform goDashY = Instantiate(dashY, graphContainer).GetComponent<RectTransform>();
            goDashY.anchoredPosition = new Vector2(0, (i * 1f / yCount) * height);
        }


        
    }

    private void CreateConnection(Vector2 dotA , Vector2 dotB)
    {
        Vector2 dir = (dotB - dotA).normalized;
        float distance = Vector2.Distance(dotA, dotB);

        GameObject go = new GameObject("Line", typeof(Image));

        go.transform.SetParent(graphContainer, false);
        go.GetComponent<Image>().color = Color.red;
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.zero;
        rect.sizeDelta = new Vector2(distance , 5f);
        rect.pivot = new Vector2(0f, 0.5f);
        rect.anchoredPosition = dotA ;
        rect.localEulerAngles = new Vector3(0f, 0f, Utils.GetAngleFromVector(dir));
        
    }

    private void OnPointerEnterHover(float x , float y)
    {
        Debug.Log($"{x} , {y}");
    }

}
