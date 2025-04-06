using UnityEngine;
using UnityEngine.UI;

public class CinematicBar : MonoBehaviour
{
    private static CinematicBar instance;

    RectTransform topBarRect, bottomBarRect;
    float targetSize, changeAmount;
    bool Activate = false;

    private void Awake()
    {
        instance = this;

        // Create top bar
        GameObject topBar = new GameObject("TopBar", typeof(Image));
        topBar.transform.SetParent(transform, false);
        topBar.GetComponent<Image>().color = Color.black;

        topBarRect = topBar.GetComponent<RectTransform>();
        topBarRect.anchorMin = new Vector2(0, 1);
        topBarRect.anchorMax = new Vector2(1, 1);
        topBarRect.pivot = new Vector2(0.5f, 1);
        topBarRect.sizeDelta = new Vector2(0, 0);

        // Create bottom bar
        GameObject bottomBar = new GameObject("BottomBar", typeof(Image));
        bottomBar.transform.SetParent(transform, false);
        bottomBar.GetComponent<Image>().color = Color.black;

        bottomBarRect = bottomBar.GetComponent<RectTransform>();
        bottomBarRect.anchorMin = new Vector2(0, 0);
        bottomBarRect.anchorMax = new Vector2(1, 0);
        bottomBarRect.pivot = new Vector2(0.5f, 0);
        bottomBarRect.sizeDelta = new Vector2(0, 0);

        
        Utils.CreateDebugButtonUI(transform, new Vector3(50 , 0) , () => ShowBar(200, 2) , Color.black);
        Utils.CreateDebugButtonUI(transform, new Vector3(-50, 0) , () => HideBar(2) , Color.white);
        
    }

    private void Update()
    {
        if (!Activate) return;

        Vector2 sizeDelta = topBarRect.sizeDelta;
        sizeDelta.y += changeAmount * Time.deltaTime;

        if (changeAmount >= 0)
        {
            if (sizeDelta.y >= targetSize)
            {
                sizeDelta.y = targetSize;
                topBarRect.sizeDelta = sizeDelta;
                bottomBarRect.sizeDelta = sizeDelta;
                Activate = false;
            }

        }
        else
        {
            if (sizeDelta.y <= targetSize)
            {
                sizeDelta.y = targetSize;
                topBarRect.sizeDelta = sizeDelta;
                bottomBarRect.sizeDelta = sizeDelta;
                Activate = false;
            }
        }

        topBarRect.sizeDelta = sizeDelta;
        bottomBarRect.sizeDelta = sizeDelta;
    }

    private void HideBar(float time)
    {
        targetSize = 0;
        changeAmount = (targetSize - topBarRect.sizeDelta.y) / time;
        Activate = true;
    }
    private void ShowBar(float targetSize, float time)
    {
        this.targetSize = targetSize;
        changeAmount = (targetSize - topBarRect.sizeDelta.y) / time;
        Activate = true;
    }
    
    public static void HideBarStatic(float time)
    {
        if (instance != null)
        {
            instance.HideBar(time);
        }
    }

    public static void ShowBarStatic(float targetSize, float time)
    {
        if (instance != null)
        {
            instance.ShowBar(targetSize, time);
        }
    }
    
}
