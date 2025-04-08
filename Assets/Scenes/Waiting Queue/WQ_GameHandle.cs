using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WQ_GameHandle : MonoBehaviour
{

    List<Vector3> waitingQueueList;

    [SerializeField] List<WQ_Guest> guestList;
    [SerializeField] Button addGuestButton , getGuestButton;

    float watingTimer = 0;
    WQ_WaitingQueue waitingQueue;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        

        Vector3 startPos = new Vector3(0 , 2.5f);
        float space = 1.5f;
        waitingQueueList = new List<Vector3>();
        for (int i = 0; i < 5; i++)
        {
            waitingQueueList.Add(startPos - new Vector3(1 , 0) * space * i);
        }

        waitingQueue = new WQ_WaitingQueue(waitingQueueList);

        waitingQueue.OnGuestArrivedAtFrontOfQueue += WaitingQueue_OnGuestArrivedAtFrontOfQueue;
        waitingQueue.OnAddGuest += WaitingQueue_OnAddGuest;

        List<Vector3> positionList = new List<Vector3>() { new Vector3(5.5f , 2f)  , new  Vector3(5.5f, -2f) };
        Vector3 exitPostion = new Vector3(0, 0);
        WQ_Destination destination = new WQ_Destination(waitingQueue, positionList , exitPostion);

    }

    private void WaitingQueue_OnAddGuest(object sender, EventArgs e)
    {
        Debug.Log("Add Guest");
    }

    private void WaitingQueue_OnGuestArrivedAtFrontOfQueue(object sender, EventArgs e)
    {
        Debug.Log("Guest Arrived");
    }

    private void Update()
    {
        watingTimer += Time.deltaTime;
        AutoAddGuest(waitingQueue, 2f);
    }

    private void AutoAddGuest(WQ_WaitingQueue waitingQueue , float maxTime)
    {
        if(watingTimer >= maxTime)
        {
            watingTimer -= maxTime;
            if (waitingQueue.CanAddGuest())
            {

                if (guestList.Count > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, guestList.Count);
                    WQ_Guest guest = guestList[randomIndex];
                    if (guest != null)
                    {
                        guestList.RemoveAt(guestList.IndexOf(guest));

                        waitingQueue.AddGuest(guest);
                        
                    }
                }

            }
        }
        
    }
}
