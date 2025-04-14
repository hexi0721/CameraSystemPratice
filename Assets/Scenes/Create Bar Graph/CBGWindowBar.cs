using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;


public class CBGWindowBar : MonoBehaviour
{

    RectTransform graph;

    RectTransform templateX , templateY , templateBar , templateDashY;

    private void Start()
    {
        graph = transform.Find("Graph").GetComponent<RectTransform>();
        templateX = graph.Find("TemplateX").GetComponent<RectTransform>();
        templateY = graph.Find("TemplateY").GetComponent<RectTransform>();
        templateBar = graph.Find("TemplateBar").GetComponent<RectTransform>();
        templateDashY = graph.Find("TemplateDashY").GetComponent<RectTransform>();

        List<float> list = new List<float>();
        int count = UnityEngine.Random.Range(11, 15);

        for (int i = 0; i < count; i++)
        {
            list.Add(UnityEngine.Random.Range(0f, 200f));
        }

        ShowGraph(list);

    }

    private void ShowGraph(List<float> list)
    {

        float maxY = float.MinValue;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] > maxY)
            {
                maxY = list[i];
            }
        }
        maxY *= 1.2f;

        CreateX(list , maxY);
        CreateY(maxY);
    }

    private void CreateX(List<float> list , float maxY)
    {
        float graphWidth = graph.rect.width;
        int xCount = list.Count;
        float xSize = graphWidth / (xCount + 1);


        for (int i = 0; i < xCount; i++)
        {
            RectTransform x = Instantiate(templateX, graph).GetComponent<RectTransform>();
            x.gameObject.SetActive(true);

            Vector3 newPosition = new Vector3(xSize + (i * xSize), 0);
            x.anchoredPosition = newPosition;

            CreateBar(newPosition.x , list[i] , maxY);

            x.GetComponent<Text>().text = (i + 1).ToString();
        }

    }

    private void CreateBar(float positonX, float value , float maxY)
    {

        float graphHeight = graph.rect.height;
        Vector2 size = templateX.rect.size;

        RectTransform bar = Instantiate(templateBar, graph).GetComponent<RectTransform>();
        bar.gameObject.SetActive(true);
        bar.sizeDelta = new Vector3(size.x, (value / maxY) * graphHeight);
        bar.anchoredPosition = new Vector3(positonX, 0);


        EventTrigger eventTrigger = bar.GetComponent<EventTrigger>();
        if (eventTrigger != null)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };
            entry.callback.AddListener((e) => { Debug.Log($"{value}"); });
            eventTrigger.triggers.Add(entry);
        }

    }

    private void CreateY(float maxY) 
    {
        float graphHeight = graph.rect.height;

        for (int i = 0; i <= 10; i++)
        {
            float normalizeValue = i * 1f / 10;
            RectTransform y = Instantiate(templateY, graph).GetComponent<RectTransform>();
            y.gameObject.SetActive(true);

            float ySize = normalizeValue * graphHeight;
            Vector3 newPosition = new Vector3(0 , ySize);
            y.anchoredPosition = newPosition;

            y.GetComponent<Text>().text = ( Mathf.RoundToInt(normalizeValue * maxY)).ToString();


            RectTransform DashY = Instantiate(templateDashY, graph).GetComponent<RectTransform>();
            DashY.gameObject.SetActive(true);
            DashY.anchoredPosition = newPosition;
        }

    }




}
