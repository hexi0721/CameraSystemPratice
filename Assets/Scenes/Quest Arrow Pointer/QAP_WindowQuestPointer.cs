using System;
using UnityEngine;
using UnityEngine.UI;

public class QAP_WindowQuestPointer : MonoBehaviour
{

    [SerializeField] Camera uiCam;
    [SerializeField] Vector3 targetPosition;
    [SerializeField] Sprite spritePointer , spriteQuestion;

    Image image;
    RectTransform pointer;

    private void Awake()
    {
        pointer = transform.Find("Pointer").GetComponent<RectTransform>();
        image = pointer.GetComponent<Image>();

        Hide();
    }

    private void Update()
    {

        Vector3 targetScreenPosition = uiCam.WorldToScreenPoint(targetPosition);
        float borderSize = 40f;
        bool isOffScreen = targetScreenPosition.x <= borderSize || targetScreenPosition.x >= Screen.width - borderSize || targetScreenPosition.y <= borderSize || targetScreenPosition.y >= Screen.height - borderSize;

        Vector3 captureTargetScreenPosition = targetScreenPosition;
        if (isOffScreen)
        {
            RotatePointerTowardsTarget();
            
            image.sprite = spritePointer;
            
            if (captureTargetScreenPosition.x <= borderSize) captureTargetScreenPosition.x = borderSize;
            if (captureTargetScreenPosition.x >= Screen.width - borderSize) captureTargetScreenPosition.x = Screen.width - borderSize;
            if (captureTargetScreenPosition.y <=  borderSize) captureTargetScreenPosition.y = borderSize;
            if (captureTargetScreenPosition.y >= Screen.height - borderSize) captureTargetScreenPosition.y = Screen.height - borderSize;

            //?? Vector3 pointerWorldPosition = uiCam.ScreenToWorldPoint(captureTargetScreenPosition);
            pointer.position = captureTargetScreenPosition;
            pointer.localPosition = new Vector3(pointer.localPosition.x, pointer.localPosition.y, 0f);
        }
        else
        {
            image.sprite = spriteQuestion;

            pointer.position = captureTargetScreenPosition;
            pointer.localPosition = new Vector3(pointer.localPosition.x, pointer.localPosition.y, 0f);
            pointer.localEulerAngles = new Vector3(0, 0, 0);
        }



    }

    private void RotatePointerTowardsTarget()
    {
        Vector3 toPosition = targetPosition;
        Vector3 fromPosition = uiCam.transform.position;
        fromPosition.z = 0;

        Vector3 dir = (toPosition - fromPosition).normalized;
        float eulerZ = Utils.GetAngleFromVector(dir);
        pointer.localEulerAngles = new Vector3(0, 0, eulerZ);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show(Vector3 targetPosition)
    {
        
        gameObject.SetActive(true);
        this.targetPosition = targetPosition;
    }
}
