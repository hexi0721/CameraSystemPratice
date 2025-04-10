using UnityEngine;

public class QAP_WindowQuestPointer : MonoBehaviour
{

    [SerializeField] Camera uiCam;
    [SerializeField] Transform tartgetPosition;
    RectTransform pointer;

    private void Start()
    {
        

        pointer = transform.Find("Pointer").GetComponent<RectTransform>();
        
    }


    private void Update()
    {
        Vector3 toPosition = tartgetPosition.position;
        Vector3 fromPosition = uiCam.transform.position;
        fromPosition.z = 0;

        Vector3 dir = (toPosition - fromPosition).normalized;
        float eulerZ = Utils.GetAngleFromVector(dir);
        pointer.localEulerAngles = new Vector3(0, 0, eulerZ);

        Vector3 targetScreenPosition = uiCam.WorldToScreenPoint(tartgetPosition.position);
        float borderSize = 80f;
        bool isOffScreen = targetScreenPosition.x <= borderSize || targetScreenPosition.x >= Screen.width - borderSize || targetScreenPosition.y <= borderSize || targetScreenPosition.y >= Screen.height - borderSize;

        if(isOffScreen)
        { 
            // pointer.gameObject.SetActive(true);
            Vector3 captureTargetScreenPosition = targetScreenPosition;

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
            // pointer.gameObject.SetActive(false);
        }



    }


}
