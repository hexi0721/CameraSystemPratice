using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WQ_GameHandle : MonoBehaviour
{

    public static WQ_GameHandle Instance { get; private set; }

    List<Vector3> waitingQueueList;
    [SerializeField] List<WQ_Guest> guestList;

    float watingTimer = 0;
    WQ_WaitingQueue waitingQueue;

    public Canvas canvas;

    private void Awake()
    {
        Instance = this;
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

        Utils.CreateDebugButtonUI(canvas.transform , new Vector3(0, -10) , waitingQueue.AddPositionDown , Color.green);
        Utils.CreateDebugButtonUI(canvas.transform, new Vector3(0, 10), waitingQueue.AddPositionUp, Color.green);
        Utils.CreateDebugButtonUI(canvas.transform, new Vector3(-10, 0), waitingQueue.AddPositionLeft, Color.green);
        Utils.CreateDebugButtonUI(canvas.transform, new Vector3(10, 0), waitingQueue.AddPositionRight, Color.green);

        Utils.CreateDebugButtonUI(canvas.transform, new Vector3(50, 0), waitingQueue.RemovePostion, Color.green);
        /*
        waitingQueue.OnGuestArrivedAtFrontOfQueue += WaitingQueue_OnGuestArrivedAtFrontOfQueue;
        waitingQueue.OnAddGuest += WaitingQueue_OnAddGuest;
        */
        List<Vector3> destinationList = new List<Vector3>() { new Vector3(5.5f , 2f)  , new  Vector3(5.5f, -2f) };
        Vector3 exitPostion = new Vector3(0, -5f);
        Utils.WorldSprtie_Create(exitPostion, new Vector3(1, 1) * .1f, Color.blue);
        WQ_Destination destination = new WQ_Destination(waitingQueue, destinationList, exitPostion , guestList);

    }
    
    private void WaitingQueue_OnGuestArrivedAtFrontOfQueue(object sender, EventArgs e)
    {
        Debug.Log("Guest Arrived At Front Of Queue");
    }

    private void WaitingQueue_OnAddGuest(object sender, EventArgs e)
    {
        Debug.Log("Add Guest");
    }

    private void Update()
    {
        WaitingQueue_AutoAddGuest(waitingQueue, 2f);
        
    }

    private void WaitingQueue_AutoAddGuest(WQ_WaitingQueue waitingQueue , float maxTime)
    {
        watingTimer += Time.deltaTime;
        if (watingTimer >= maxTime)
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
