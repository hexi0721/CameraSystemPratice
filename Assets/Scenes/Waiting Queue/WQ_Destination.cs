using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WQ_Destination
{
    WQ_WaitingQueue watingQueue;
    List<DestinationPosition> destinationPositionList;
    Vector3 exitPostion;

    public WQ_Destination(WQ_WaitingQueue watingQueue , List<Vector3> postionList, Vector3 exitPostion)
    {
        this.watingQueue = watingQueue;
        destinationPositionList = new List<DestinationPosition>();
        this.exitPostion = exitPostion;

        foreach (Vector3 postion in postionList)
        {
            Utils.WorldSprtie_Create(postion, new Vector3(1, 1) * .1f, Color.white);
            destinationPositionList.Add(new DestinationPosition() { destinationPosition = postion });
        }

        watingQueue.OnGuestArrivedAtFrontOfQueue += watingQueue_OnGuestArrivedAtFrontOfQueue;
        this.exitPostion = exitPostion;
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
            WQ_Guest guest = watingQueue.GetGuest();

            if (guest != null)
            {
                destinationPosition.SetGuest(guest);
                guest.MoveTo(destinationPosition.destinationPosition , () => 
                {
                    guest.DoSomething(() => 
                    {
                        destinationPosition.ClearGuest(); 
                        guest.MoveTo(exitPostion, () => 
                        {
                            TrySendGuestToDestination();
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
        public WQ_Guest guest;
        public Vector3 destinationPosition;

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
