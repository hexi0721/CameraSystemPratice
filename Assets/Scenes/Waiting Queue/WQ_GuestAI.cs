using UnityEngine;

public class WQ_GuestAI
{

    WQ_WaitingQueue waitingQueue;
    WQ_Guest guest;

    public WQ_GuestAI(WQ_WaitingQueue waitingQueue , WQ_Guest guest , Vector3 entrancePosition)
    {
        this.waitingQueue = waitingQueue;
        this.guest = guest;

        guest.MoveTo(entrancePosition , () => waitingQueue.GuestRequestSetQueuePosition(this));
    }

    public void SetQueuePosition(Vector3 position)
    {
        guest.MoveTo(position);
    }

}
