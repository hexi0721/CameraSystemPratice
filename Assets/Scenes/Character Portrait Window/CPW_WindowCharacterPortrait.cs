using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class CPW_WindowCharacterPortrait : MonoBehaviour
{

    [SerializeField] private Transform pf_Window_CharacterPortrait;
    private Dictionary<Transform, WindowPortrait> windowPortraitDict;

    private void Awake()
    {
        windowPortraitDict = new Dictionary<Transform, WindowPortrait>();
    }

    private void LateUpdate()
    {
        if(windowPortraitDict.Count <= 0)
        {
            return;
        }
         
        foreach(var kvp in new Dictionary<Transform, WindowPortrait>(windowPortraitDict))
        {
            WindowPortrait windowPortrait = kvp.Value;

            if (windowPortrait.LateUpdte())
            {
                Transform followTransform = windowPortrait.followrPosition;
                windowPortraitDict.Remove(followTransform);
                
            }
        }
         
    }


    public void CreateWindowPortrait(Transform followrPosition)
    {
        if (windowPortraitDict == null)
        {
            windowPortraitDict = new Dictionary<Transform, WindowPortrait>();
        }

        if(!windowPortraitDict.ContainsKey(followrPosition)){
            Transform TF = Instantiate(pf_Window_CharacterPortrait, transform);

            WindowPortrait windowPortrait = new WindowPortrait(TF, followrPosition);
            windowPortraitDict.Add(followrPosition , windowPortrait);
        }

    }

    public class WindowPortrait
    {
        private Transform TF;
        public Transform followrPosition { get; private set; }
        private Transform cam;

        private bool IsDestroy;


        public WindowPortrait(Transform TF , Transform followrPosition)
        {
            this.TF = TF;
            this.followrPosition = followrPosition;
            
            cam = TF.Find("Cam_CharacterPortrait");

            Button Btn_Close = TF.Find("Btn_Close").GetComponent<Button>();
            Btn_Close.onClick.AddListener(DestroySelfAction);

            IsDestroy = false;

            RenderTexture renderTexture = new RenderTexture(512 , 512 , 16);
            cam.gameObject.GetComponent<Camera>().targetTexture = renderTexture;

            RawImage rawImage = TF.Find("RawImage").GetComponent<RawImage>();
            rawImage.texture = renderTexture;

            RectTransform rectTransform = TF.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(Random.Range(-100f , 100f) , Random.Range(-100f, 100f));


        }

        public bool LateUpdte()
        {
            cam.position = new Vector3(followrPosition.position.x, followrPosition.position.y, Camera.main.transform.position.z);

            if (IsDestroy)
            {
                Destroy(TF.gameObject);
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
