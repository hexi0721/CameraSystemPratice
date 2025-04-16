using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class CPW_WindowCharacterPortrait : MonoBehaviour
{

    [SerializeField] private Transform pf_Window_CharacterPortrait;
    private Dictionary<CPW_AI, WindowPortrait> windowPortraitDict;

    private void Awake()
    {
        windowPortraitDict = new Dictionary<CPW_AI, WindowPortrait>();
    }

    private void LateUpdate()
    {
        if(windowPortraitDict.Count <= 0)
        {
            return;
        }
         
        foreach(var kvp in new Dictionary<CPW_AI, WindowPortrait>(windowPortraitDict))
        {
            WindowPortrait windowPortrait = kvp.Value;

            if (windowPortrait.LateUpdte())
            {
                CPW_AI cpw_AI = windowPortrait.cpw_AI;
                windowPortraitDict.Remove(cpw_AI);
                
            }
        }
         
    }


    public void CreateWindowPortrait(CPW_AI cpw_AI)
    {
        if (windowPortraitDict == null)
        {
            windowPortraitDict = new Dictionary<CPW_AI, WindowPortrait>();
        }

        if(!windowPortraitDict.ContainsKey(cpw_AI)){
            Transform TF = Instantiate(pf_Window_CharacterPortrait, transform);

            WindowPortrait windowPortrait = new WindowPortrait(TF, cpw_AI);
            windowPortraitDict[cpw_AI] = windowPortrait;
        }

    }

    public class WindowPortrait
    {
        public CPW_AI cpw_AI { get; private set; }
        private Transform transform;
        public Transform followrPosition;
        RectTransform windowPortraitRectTransform;
        float portraitRectWidth;
        float portraitRectHeight;

        private Transform cam;

        private bool IsDestroy;


        public WindowPortrait(Transform transform , CPW_AI cpw_AI)
        {
            this.transform = transform;
            followrPosition = cpw_AI.transform;
            
            cam = transform.Find("Cam_CharacterPortrait");

            Button Btn_Close = transform.Find("Btn_Close").GetComponent<Button>();
            Btn_Close.onClick.AddListener(DestroySelfAction);

            IsDestroy = false;

            RenderTexture renderTexture = new RenderTexture(512 , 512 , 16);
            cam.gameObject.GetComponent<Camera>().targetTexture = renderTexture;

            RawImage rawImage = transform.Find("RawImage").GetComponent<RawImage>();
            rawImage.texture = renderTexture;

            windowPortraitRectTransform = transform.GetComponent<RectTransform>();
            portraitRectWidth = windowPortraitRectTransform.rect.width;
            portraitRectHeight = windowPortraitRectTransform.rect.height;



        }

        public bool LateUpdte()
        {
            Vector2 screenPos = Camera.main.WorldToScreenPoint(followrPosition.position);
            Vector2 newPos = Vector2.zero;

            if (screenPos.x <= Screen.width / 2)
            {
                newPos += new Vector2(0, 0);
            }
            else if(screenPos.x > Screen.width / 2)
            {
                newPos += new Vector2(portraitRectWidth, 0);
            }

            if (screenPos.y <= Screen.height / 2)
            {
                newPos += new Vector2(0, -portraitRectHeight);
            }
            else if (screenPos.y > Screen.height / 2)
            {
                newPos += new Vector2(0, 0);
            }

            windowPortraitRectTransform.position = newPos + screenPos;

            cam.position = new Vector3(followrPosition.position.x, followrPosition.position.y, Camera.main.transform.position.z);


            if (IsDestroy)
            {
                Destroy(transform.gameObject);
                return true;
            }

            return false;
        }

        private void DestroySelfAction()
        {
            IsDestroy = true;
        }

    }

}
