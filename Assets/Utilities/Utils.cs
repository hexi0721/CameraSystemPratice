using UnityEngine;
using UnityEngine.UI;

public static class Utils
{
    public static Vector3 GetMouseWorldPos()
    {
        Vector3 mouserPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return mouserPos;
    }

    public static Vector3 GetMouseWorldPosZeroZ()
    {
        Vector3 mouserPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouserPos.z = 0;
        return mouserPos;
    }
    
    public static float GetAngleFromVector(Vector3 dir)
    {
        float x = dir.x;
        float y = dir.y;
        float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;

        return (angle < 0) ? angle + 360 : angle; 
    }
    
    public static void WorldSprtie_Create(Vector3 postion , Vector3 localScale , Color color)
    {
        GameObject gameObject = new GameObject("Sprite" , typeof(SpriteRenderer));
        gameObject.transform.position = postion;
        gameObject.transform.localScale = localScale;

        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Square");
        gameObject.GetComponent<SpriteRenderer>().color = color;

    }

    public static void CreateDebugButtonUI(Transform parent , Vector3 anchoredPosition, System.Action ClickFunc , Color color)
    {
        GameObject buttonObj = new GameObject("DebugButton", typeof(RectTransform), typeof(UnityEngine.UI.Button) , typeof(Image));
        buttonObj.GetComponent<Image>().color = color;


        buttonObj.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        buttonObj.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 10);
        buttonObj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        buttonObj.GetComponent<RectTransform>().SetParent(parent, false);
        buttonObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ClickFunc());

    }
}