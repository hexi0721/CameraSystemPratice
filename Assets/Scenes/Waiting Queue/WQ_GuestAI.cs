using UnityEngine;

public class WQ_GuestAI
{

    enum State
    {
        GoingToEntrancePosition,
        WaitingInMiddleOfQueue,

    }

    private WQ_WaitingQueue waitingQueue;
    private WQ_Guest guest;
    private State state;

    public WQ_GuestAI(WQ_WaitingQueue waitingQueue , WQ_Guest guest , Vector3 entrancePosition)
    {
        this.waitingQueue = waitingQueue;
        this.guest = guest;

        state = State.GoingToEntrancePosition;
        guest.MoveTo(entrancePosition , () => {
            state = State.WaitingInMiddleOfQueue;
            waitingQueue.GuestRequestSetQueuePosition(this); 
        });
    }

    public WQ_Guest GetGuest()
    {
        return guest;
    }

    public void SetQueuePosition(Vector3 position , WQ_WaitingQueue waitingQueue)
    {
        guest.MoveTo(position , () => { waitingQueue.GuestArrivedAtQueueOfPositon(this); });
    }

    public bool IsWaitingInMiddleOfQueue() 
    {
        return state == State.WaitingInMiddleOfQueue;
    }

}
