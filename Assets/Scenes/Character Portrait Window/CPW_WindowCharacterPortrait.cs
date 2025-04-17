using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;

public class CPW_WindowCharacterPortrait : MonoBehaviour
{

    [SerializeField] private Transform pf_Window_CharacterPortrait;
    private Dictionary<CPW_CharacterScriptableObject, WindowPortrait> windowPortraitDict;

    private void Awake()
    {
        windowPortraitDict = new Dictionary<CPW_CharacterScriptableObject, WindowPortrait>();
    }

    private void LateUpdate()
    {
        if(windowPortraitDict.Count <= 0)
        {
            return;
        }
         
        foreach(var kvp in new Dictionary<CPW_CharacterScriptableObject, WindowPortrait>(windowPortraitDict))
        {
            WindowPortrait windowPortrait = kvp.Value;

            if (windowPortrait.LateUpdte())
            {

                windowPortraitDict.Remove(windowPortrait.cpw_CharacterScriptableObject);

            }
        }
         
    }


    public void CreateWindowPortrait(CPW_CharacterScriptableObject cpw_CharacterScriptableObject)
    {
        if (windowPortraitDict == null)
        {
            windowPortraitDict = new Dictionary<CPW_CharacterScriptableObject, WindowPortrait>();
        }

        if(!windowPortraitDict.ContainsKey(cpw_CharacterScriptableObject)){
            Transform TF = Instantiate(pf_Window_CharacterPortrait, transform);

            WindowPortrait windowPortrait = new WindowPortrait(TF, cpw_CharacterScriptableObject);
            windowPortraitDict[cpw_CharacterScriptableObject] = windowPortrait;
        }

    }

    public class WindowPortrait
    {
        Transform TF; // 自己

        public CPW_CharacterScriptableObject cpw_CharacterScriptableObject { get; private set; } // 要追隨的AI character 的 ScriptableObject
        Transform followrPosition ;
         
        Transform cam;
        bool IsDestroy;

        RectTransform windowPortraitRectTransform;
        float portraitRectWidth , portraitRectHeight;

        Text levelText , atkText, defText;
        RectTransform Hp, Exp;

        public WindowPortrait(Transform TF , CPW_CharacterScriptableObject cpw_CharacterScriptableObject)
        {
            this.TF = TF;

            this.cpw_CharacterScriptableObject = cpw_CharacterScriptableObject;
            followrPosition = cpw_CharacterScriptableObject.TF;

            cam = TF.Find("Cam_CharacterPortrait");

            Button Btn_Close = TF.Find("Btn_Close").GetComponent<Button>();
            Btn_Close.onClick.AddListener(() => { IsDestroy = true; });

            IsDestroy = false;

            RenderTexture renderTexture = new RenderTexture(512 , 512 , 16);
            cam.gameObject.GetComponent<Camera>().targetTexture = renderTexture;

            RawImage rawImage = TF.Find("RawImage").GetComponent<RawImage>();
            rawImage.texture = renderTexture;

            windowPortraitRectTransform = TF.GetComponent<RectTransform>();
            portraitRectWidth = windowPortraitRectTransform.rect.width;
            portraitRectHeight = windowPortraitRectTransform.rect.height;

            Text nameText= TF.Find("Name").GetComponent<Text>();
            nameText.text = cpw_CharacterScriptableObject.名字;
             
            atkText = TF.Find("ATK").GetComponent<Text>();
            defText = TF.Find("DEF").GetComponent<Text>();
            levelText = TF.Find("Level").GetComponent<Text>();
            atkText.text = $"攻擊 : {cpw_CharacterScriptableObject.攻擊.ToString()}";
            defText.text = $"防禦 : {cpw_CharacterScriptableObject.防禦.ToString()}";
            levelText.text = $"LV.{cpw_CharacterScriptableObject.Level.ToString()}";


            Hp = TF.Find("HPbar/HP").GetComponent<RectTransform>();
            Exp = TF.Find("EXPbar/EXP").GetComponent<RectTransform>();

            cpw_CharacterScriptableObject.OnLevelUpEvent += cpw_CharacterScriptableObject_OnLevelUpEvent;


            cpw_CharacterScriptableObject.OnExpChangeEvent += cpw_CharacterScriptableObject_OnExpChangeEvent;

        }

        private void cpw_CharacterScriptableObject_OnExpChangeEvent(object sender, EventArgs e)
        {
            Vector3 expScale = new Vector3(cpw_CharacterScriptableObject.GetEXPNormalized(), 1, 1);
            Exp.localScale = expScale;
        }

        private void cpw_CharacterScriptableObject_OnLevelUpEvent(object sender , EventArgs e)
        {
            atkText.text = $"攻擊 : {cpw_CharacterScriptableObject.攻擊.ToString()}";
            defText.text = $"防禦 : {cpw_CharacterScriptableObject.防禦.ToString()}";
            levelText.text = $"LV.{cpw_CharacterScriptableObject.Level.ToString()}";
        }

        public bool LateUpdte()
        {
            if (IsDestroy)
            {
                cpw_CharacterScriptableObject.OnLevelUpEvent -= cpw_CharacterScriptableObject_OnLevelUpEvent;
                cpw_CharacterScriptableObject.OnExpChangeEvent -= cpw_CharacterScriptableObject_OnExpChangeEvent;

                Destroy(TF.gameObject);
                return true;
            }

            校準鏡頭();
            
            Vector3 hpScale = new Vector3(cpw_CharacterScriptableObject.GetHPNormalized(), 1, 1);
            Hp.localScale = hpScale;

            return false;
        }
        private void 校準鏡頭()
        {
            Vector2 screenPos = Camera.main.WorldToScreenPoint(followrPosition.position);
            Vector2 newPos = Vector2.zero;

            if (screenPos.x <= Screen.width / 2)
            {
                newPos += new Vector2(-portraitRectWidth / 2, 0);
            }
            else if (screenPos.x > Screen.width / 2)
            {
                newPos += new Vector2(portraitRectWidth / 2, 0);
            }

            if (screenPos.y <= Screen.height / 2)
            {
                newPos += new Vector2(0, -portraitRectHeight / 2);
            }
            else if (screenPos.y > Screen.height / 2)
            {
                newPos += new Vector2(0, portraitRectHeight / 2);
            }

            windowPortraitRectTransform.position = newPos + screenPos;

            cam.position = new Vector3(followrPosition.position.x, followrPosition.position.y, Camera.main.transform.position.z);
        }

    }


}
