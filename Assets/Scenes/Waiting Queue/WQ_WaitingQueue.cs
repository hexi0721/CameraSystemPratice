using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;
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
            entrance = waitingQueueList[^1] ;
        }
        else
        {
            Vector3 dir = (waitingQueueList[^1] - waitingQueueList[^2]).normalized;
            entrance = waitingQueueList[^1] + 1.5f * dir;
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
        AddPostion(waitingQueueList[^1] + new Vector3(0, -1) * POSITION_SIZE);
    }
    public void AddPositionUp()
    {
        AddPostion(waitingQueueList[^1] + new Vector3(0, 1) * POSITION_SIZE);
    }
    public void AddPositionLeft()
    {
        AddPostion(waitingQueueList[^1] + new Vector3(-1, 0) * POSITION_SIZE);
    }
    public void AddPositionRight()
    {
        AddPostion(waitingQueueList[^1] + new Vector3(1, 0) * POSITION_SIZE);
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
        
        OnAddGuest?.Invoke(this, EventArgs.Empty);
    }

    public void GuestRequestSetQueuePosition(WQ_GuestAI guestAI)
    {
        for(int i = 0; i < waitingQueueList.Count; i++)
        {
            if (guestAIList[i] == guestAI)
            {
                break;
            }
            else
            {
                if(!guestAIList[i].IsWaitingInMiddleOfQueue())   // 當有人還沒經過入口，新的客人將取代他的位置 所以兩個位置要交換
                {
                    guestAIList[guestAIList.IndexOf(guestAI)] = guestAIList[i];
                    guestAIList[i] = guestAI;
                    break;
                }
            }
        }

        guestAI.SetQueuePosition(waitingQueueList[guestAIList.IndexOf(guestAI)] , this);
    }

    public WQ_Guest GetGuest()
    {
        if(guestAIList.Count == 0)
        {
            return null;
        }
        else
        {
            
            WQ_Guest guest = guestAIList[0].GetGuest();
            
            guestAIList.RemoveAt(0);
            RelocateAllGuest();
            return guest;
            

        }
    }

    private void RelocateAllGuest()
    {
        foreach(WQ_GuestAI guestAI in guestAIList)
        {
            if(guestAI.IsWaitingInMiddleOfQueue())
            {
                guestAI.SetQueuePosition(waitingQueueList[guestAIList.IndexOf(guestAI)], this);
            }
        }
    }

    public void GuestArrivedAtQueueOfPositon(WQ_GuestAI guestAI)
    {
        if (guestAIList.Count > 0 && guestAI == guestAIList[0])
        {
            OnGuestArrivedAtFrontOfQueue?.Invoke(this, EventArgs.Empty);
        }
    }

}
