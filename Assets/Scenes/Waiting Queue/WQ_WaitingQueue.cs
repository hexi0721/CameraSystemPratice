using System.Collections.Generic;
using UnityEngine;
using System;
public class WQ_WaitingQueue
{
    public event EventHandler OnAddGuest;
    public event EventHandler OnGuestArrivedAtFrontOfQueue;

    const float POSITION_SIZE = 1.5f;

    List<WQ_Guest> guestList;
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
        
        guestList = new List<WQ_Guest>();
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
        if(guestList.Count < waitingQueueList.Count)
        {
            waitingQueueList.RemoveAt(waitingQueueList.Count - 1);
            CalculateEntrancePosition();
        }
    }


    public bool CanAddGuest()
    {
        return guestList.Count < waitingQueueList.Count;
    }

    public void AddGuest(WQ_Guest guest)
    {
        guestList.Add(guest);
        guest.MoveTo(waitingQueueList[guestList.IndexOf(guest)] , () => { GuestArrivedAtQueueOfPositon(guest); });
        OnAddGuest?.Invoke(this, EventArgs.Empty);
    }

    public WQ_Guest GetGuest()
    {
        if(guestList.Count == 0)
        {
            return null;
        }
        else
        {
            WQ_Guest guest = guestList[0];
            
            guestList.RemoveAt(guestList.IndexOf(guest));
            RelocateAllGuest();
            return guest;
        }
    }

    private void RelocateAllGuest()
    {
        foreach(WQ_Guest guest in guestList)
        {
            guest.MoveTo(waitingQueueList[guestList.IndexOf(guest)] , () => { GuestArrivedAtQueueOfPositon(guest); });
        }
    }

    private void GuestArrivedAtQueueOfPositon(WQ_Guest guest)
    {
        if (guestList.Count > 0 &&  guest == guestList[0])
        {
            OnGuestArrivedAtFrontOfQueue?.Invoke(this, EventArgs.Empty);
        }
    }

}
