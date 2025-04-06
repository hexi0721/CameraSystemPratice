using System.Collections.Generic;
using UnityEngine;

public class WQ_WaitingQueue
{

    List<WQ_Guest> guestList;
    List<Vector3> waitingQueueList;
    Vector3 entrance;

    public WQ_WaitingQueue(List<Vector3> waitingQueueList)
    {
        this.waitingQueueList = waitingQueueList;
        //entrance = waitingQueueList[0] + new Vector3(1.5f , 0); 
        /*
        foreach(var waitingQueue in waitingQueueList)
        {
            Utils.WorldSprtie_Create(waitingQueue, new Vector3(1, 1), Color.white);
        }

        Utils.WorldSprtie_Create(entrance, new Vector3(1, 1), Color.red);
        */
        guestList = new List<WQ_Guest>();
    }

    
    public bool CanAddGuest()
    {
        return guestList.Count < waitingQueueList.Count;
    }

    public void AddGuest(WQ_Guest guest)
    {
        guestList.Add(guest);
        
        guest.MoveTo(waitingQueueList[guestList.IndexOf(guest)]);

        

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
            guest.MoveTo(waitingQueueList[guestList.IndexOf(guest)]);
        }
    }

    
}
