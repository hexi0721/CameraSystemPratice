using System.Collections.Generic;
using UnityEngine;
using System;
public class WQ_WaitingQueue
{
    public event EventHandler OnAddGuest;
    public event EventHandler OnGuestArrivedAtFrontOfQueue;

    const float POSITION_SIZE = 1.5f;

    List<WQ_GuestAI> guestAIList;
    List<Vector3> waitingQueueList;
    Vector3 entrance;

    public WQ_WaitingQueue(List<Vector3> waitingQueueList)
    {
        this.waitingQueueList = waitingQueueList;

        CalculateEntrancePosition();

        foreach (var waitingQueue in waitingQueueList)
        {
            Utils.WorldSprtie_Create(waitingQueue, new Vector3(1, 1) * .1f, Color.white);
        }
        
        guestAIList = new List<WQ_GuestAI>();
    }

    private void CalculateEntrancePosition()
    {
        if (waitingQueueList.Count <= 1)
        {
            entrance = waitingQueueList[waitingQueueList.Count - 1] ;
        }
        else
        {
            Vector3 dir = (waitingQueueList[waitingQueueList.Count - 1] - waitingQueueList[waitingQueueList.Count - 2]).normalized;
            entrance = waitingQueueList[waitingQueueList.Count - 1] + 1.5f * dir;
        }

        
    }

    public void AddPostion(Vector3 position)
    {
        waitingQueueList.Add(position);
        Utils.WorldSprtie_Create(position, new Vector3(1, 1) * .1f, Color.white);
        CalculateEntrancePosition();
        Utils.WorldSprtie_Create(entrance, new Vector3(1, 1) * .1f, Color.red);
    }

    public void AddPositionDown()
    {
        AddPostion(waitingQueueList[waitingQueueList.Count - 1] + new Vector3(0, -1) * POSITION_SIZE);
    }
    public void AddPositionUp()
    {
        AddPostion(waitingQueueList[waitingQueueList.Count - 1] + new Vector3(0, 1) * POSITION_SIZE);
    }
    public void AddPositionLeft()
    {
        AddPostion(waitingQueueList[waitingQueueList.Count - 1] + new Vector3(-1, 0) * POSITION_SIZE);
    }
    public void AddPositionRight()
    {
        AddPostion(waitingQueueList[waitingQueueList.Count - 1] + new Vector3(1, 0) * POSITION_SIZE);
    }

    public void RemovePostion()
    {
        if(guestAIList.Count < waitingQueueList.Count)
        {
            waitingQueueList.RemoveAt(waitingQueueList.Count - 1);
            CalculateEntrancePosition();
        }
    }


    public bool CanAddGuest()
    {
        return guestAIList.Count < waitingQueueList.Count;
    }


    public void AddGuest(WQ_Guest guest)
    {
        
        WQ_GuestAI guestAI = new WQ_GuestAI(this , guest, entrance);
        guestAIList.Add(guestAI);
        // guest.MoveTo(waitingQueueList[guestAIList.IndexOf(guest)] , () => { GuestArrivedAtQueueOfPositon(guest); });
        OnAddGuest?.Invoke(this, EventArgs.Empty);
    }

    public void GuestRequestSetQueuePosition(WQ_GuestAI guestAI)
    {
        guestAI.SetQueuePosition(waitingQueueList[guestAIList.IndexOf(guestAI)]);
    }

    public WQ_Guest GetGuest()
    {
        if(guestAIList.Count == 0)
        {
            return null;
        }
        else
        {
            /*
            WQ_GuestAI guest = guestAIList[0];
            
            guestAIList.RemoveAt(guestAIList.IndexOf(guest));
            RelocateAllGuest();
            return guest;
            */

            return null;
        }
    }

    private void RelocateAllGuest()
    {/*
        foreach(WQ_GuestAI guest in guestAIList)
        {
            guest.MoveTo(waitingQueueList[guestAIList.IndexOf(guest)] , () => { GuestArrivedAtQueueOfPositon(guest); });
        }*/
    }

    private void GuestArrivedAtQueueOfPositon(WQ_GuestAI guest)
    {/*
        if (guestAIList.Count > 0 &&  guest == guestAIList[0])
        {
            OnGuestArrivedAtFrontOfQueue?.Invoke(this, EventArgs.Empty);
        }*/
    }

}
