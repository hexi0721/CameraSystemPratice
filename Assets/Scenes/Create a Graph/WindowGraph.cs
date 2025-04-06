using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WindowGraph : MonoBehaviour
{

    RectTransform graphContainer;
    [SerializeField] Sprite circleSprite;

    private void Awake()
    {
        graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();

        List<int> valueList = new List<int>();
        for (int i = 0; i < 10; i++)
        {
            valueList.Add(Random.Range(0, 100));
        }
        
        ShowGraph(valueList);
    }


    private RectTransform CreateCircle(Vector2 anchoredPosition)
    {
        GameObject circle = new GameObject("Circle" , typeof(Image));
        circle.transform.SetParent(graphContainer, false);
        circle.GetComponent<Image>().sprite = circleSprite;

        RectTransform rect = circle.GetComponent<RectTransform>();
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = new Vector2(20f, 20f);
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.zero;

        return rect;
    }

    private void ShowGraph(List<int> valueList)
    {
        float height = graphContainer.GetComponent<RectTransform>().sizeDelta.y;
        float yMax = 100f;
        float xSize = 100f;

        RectTransform rect = null;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPos = xSize + i * xSize;
            float yPos = (valueList[i] / yMax) * height;
            RectTransform circleRect = CreateCircle(new Vector2(xPos, yPos));


            if (rect != null)
            {
                CreateConnection(rect.anchoredPosition , circleRect.anchoredPosition);
            }

            rect = circleRect;
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

}
