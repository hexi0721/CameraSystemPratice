using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WQ_Destination
{
    WQ_WaitingQueue watingQueue;
    List<DestinationPosition> destinationPositionList;
    Vector3 exitPostion;
    List<WQ_Guest> guestList;

    public WQ_Destination(WQ_WaitingQueue watingQueue , List<Vector3> postionList, Vector3 exitPostion , List<WQ_Guest> guestList)
    {
        this.watingQueue = watingQueue;
        
        destinationPositionList = new List<DestinationPosition>();
        foreach (Vector3 postion in postionList)
        {
            Utils.WorldSprtie_Create(postion, new Vector3(1, 1) * .1f, Color.white);
            destinationPositionList.Add(new DestinationPosition(postion));
        }

        this.exitPostion = exitPostion;
        this.guestList = guestList;

        watingQueue.OnGuestArrivedAtFrontOfQueue += watingQueue_OnGuestArrivedAtFrontOfQueue;
        
    }

    private void watingQueue_OnGuestArrivedAtFrontOfQueue(object sender, System.EventArgs e)
    {
        TrySendGuestToDestination();
    }

    private void TrySendGuestToDestination()
    {
        DestinationPosition destinationPosition = GetEmpty();
        if (destinationPosition != null)
        {
            WQ_Guest guest = watingQueue.GetGuest(); // 會觸發 OnGuestArrivedAtFrontOfQueue 事件

            if (guest != null)
            {
                destinationPosition.SetGuest(guest);
                guest.MoveTo(destinationPosition.GetPosition(), () => 
                {
                    guest.DoSomething(() => 
                    {
                        destinationPosition.ClearGuest(); 
                        guest.MoveTo(exitPostion, () => 
                        {
                            TrySendGuestToDestination();
                            guestList.Add(guest);
                        });

                    });
                });
                
            }

        }

    }


    private DestinationPosition GetEmpty()
    {
        foreach(var destinationPosition in destinationPositionList)
        {
            if (destinationPosition.IsEmpty())
            {
                return destinationPosition;
            }
        }
        return null;
    }

    private class DestinationPosition
    {
        private WQ_Guest guest;
        private Vector3 destinationPosition;

        public DestinationPosition(Vector3 destinationPosition)
        {
            this.destinationPosition = destinationPosition;
        }

        public bool IsEmpty()
        {
            return guest == null;
        }

        public void SetGuest(WQ_Guest guest)
        {
            this.guest = guest;
        }

        public Vector3 GetPosition()
        {
            return destinationPosition;
        }

        public void ClearGuest()
        {
            guest = null;
        }
    }


}
