using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class QAP_WindowQuestPointer : MonoBehaviour
{

    [SerializeField] Camera uiCam;
    
    [SerializeField] Sprite spritePointer;
    [SerializeField] Sprite spriteQuestion;
    [SerializeField] Sprite spriteAnswer;

    private List<QuestPointer> questPointerList;


    private void Start()
    {
        questPointerList = new List<QuestPointer>();
    }

    private void Update()
    {

        foreach (QuestPointer questPointer in new List<QuestPointer>(questPointerList))
        {
            if(questPointer.Update())
            {
                questPointerList.Remove(questPointer);
            }
        }

    }

    public QuestPointer CreatePointer(Transform player ,  Vector3 targetPosition , int stat , Color color)
    {

        Transform pointerDuplicate = Instantiate(transform.Find("PointerTemplate"), transform);
        pointerDuplicate.gameObject.SetActive(true);
        QuestPointer questPointer = new QuestPointer(player, targetPosition, pointerDuplicate, spritePointer, spriteQuestion , spriteAnswer, color , stat , uiCam);
        
        questPointerList.Add(questPointer);
        return questPointer;
    }

    public class QuestPointer
    {
        Transform player;
        Vector3 targetPosition;
        Transform transform;
        Sprite spritePointer;
        Sprite spriteInstruct;
        
        Sprite spriteAnswer;
        
        RectTransform rect;
        Image image;
        Color color;
        int stat;
        Camera uiCam;

        public QuestPointer(Transform player ,  Vector3 targetPosition , Transform transform , Sprite spritePointer, Sprite spriteQuestion , Sprite spriteAnswer, Color color, int stat , Camera uiCam)
        {
            this.player = player;
            this.targetPosition = targetPosition;
            this.transform = transform;
            this.spritePointer = spritePointer;
            
            this.spriteAnswer = spriteAnswer;

            this.uiCam = uiCam;
            this.stat = stat;

            if(stat == 0)
            {
                spriteInstruct = spriteQuestion;
            }
            else if (stat == 1)
            {
                spriteInstruct = spriteAnswer;
            }

            this.color = color;
            rect = transform.gameObject.GetComponent<RectTransform>();
            image = rect.GetComponent<Image>();
            image.color = color;
        }


        public bool Update()
        {
            
            Vector3 targetScreenPosition = uiCam.WorldToScreenPoint(targetPosition);
            
            float borderSize = 40f;
            bool isOffScreen = targetScreenPosition.x <= borderSize || targetScreenPosition.x >= Screen.width - borderSize || targetScreenPosition.y <= borderSize || targetScreenPosition.y >= Screen.height - borderSize;

            Vector3 captureTargetScreenPosition = targetScreenPosition;
            
            if (isOffScreen)
            {
                
                RotatePointerTowardsTarget();
                
                image.sprite = spritePointer;

                captureTargetScreenPosition = new Vector3(Mathf.Clamp(captureTargetScreenPosition.x, borderSize, Screen.width - borderSize), Mathf.Clamp(captureTargetScreenPosition.y, borderSize, Screen.height - borderSize));

                captureTargetScreenPosition = uiCam.ScreenToWorldPoint(captureTargetScreenPosition);
                rect.position = captureTargetScreenPosition;
                rect.localPosition = new Vector3(rect.localPosition.x, rect.localPosition.y, 0f);
            }
            else
            {
                image.sprite = spriteInstruct;
                captureTargetScreenPosition = uiCam.ScreenToWorldPoint(captureTargetScreenPosition);
                rect.position = captureTargetScreenPosition;
                rect.localPosition = new Vector3(rect.localPosition.x, rect.localPosition.y, 0f);
                rect.localEulerAngles = new Vector3(0, 0, 0);
            }

            

            return CalculateDistance();
        }


        private void RotatePointerTowardsTarget()
        {
            Vector3 toPosition = targetPosition;
            Vector3 fromPosition = uiCam.transform.position;
            fromPosition.z = 0;

            Vector3 dir = (toPosition - fromPosition).normalized;
            float eulerZ = Utils.GetAngleFromVector(dir);
            rect.localEulerAngles = new Vector3(0, 0, eulerZ);

        }


        public bool CalculateDistance()
        {

            float distance = Vector3.Distance(targetPosition, player.position);
            if (distance > 2f && distance <= 4f)
            {
                color.a = 0f;
                image.color = color;

            }
            else if(distance > 4f)
            {
                color.a = 1f;
                image.color = color;
            }
            else if(distance <= 2f && QuestFinishTurnToAnswerPoinetr())
            {

                Destroy(transform.gameObject);
                return true;
            }

            return false;
        }


        private bool QuestFinishTurnToAnswerPoinetr()
        {

            if (stat == 1)
            {
                return true;
            }

            stat = 1;
            Vector3 newTargetPosition = new Vector3(UnityEngine.Random.Range(-18f , 18f) , UnityEngine.Random.Range(-10f , 10f));
            targetPosition = newTargetPosition;
            spriteInstruct = spriteAnswer;

            return false;
        }

    }
}
