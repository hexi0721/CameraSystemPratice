using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;


public class QAP_GameHandler : MonoBehaviour
{
    [SerializeField] List<Vector3> targetPositionList;
    [SerializeField] QAP_WindowQuestPointer windowQuestPointer;

    [SerializeField] List<Color> color;

    [SerializeField] Transform 玩家;

    private void Start()
    {

        int count = Random.Range(3, 8);

        for (int i = 0; i < count; i++)
        {
            targetPositionList.Add(new Vector3(Random.Range(-15f, 15f), Random.Range(-9f, 9f)));
        }

        windowQuestPointer.CreatePointer(玩家 , targetPositionList[^1], Random.Range(0, 1), color[1]);
        windowQuestPointer.CreatePointer(玩家 , targetPositionList[^2], Random.Range(0, 1), color[2]);


        foreach (Vector3 targetPosition in targetPositionList)
        {
            if(targetPosition == targetPositionList[^1] || targetPosition == targetPositionList[^2])
            {
                break;
            }

            windowQuestPointer.CreatePointer(玩家 , targetPosition, Random.Range(0, 1), color[0]);
        }


    }

    private void Update()
    {
        
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        if(v != 0 || h != 0)
        {
            玩家.position += new Vector3(h, v) * Time.deltaTime * 5f;
        }

    }

}
