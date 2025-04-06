using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WQ_GameHandle : MonoBehaviour
{
    
    List<Vector3> waitingQueueList;

    [SerializeField] List<WQ_Guest> guestList;
    [SerializeField] Button addGuestButton , getGuestButton;

    private void Start()
    {
        

        Vector3 startPos = new Vector3(0 , 2.5f);
        float space = 1.5f;
        waitingQueueList = new List<Vector3>();
        for (int i = 0; i < 5; i++)
        {
            waitingQueueList.Add(startPos - new Vector3(1 , 0) * space * i);
        }

        WQ_WaitingQueue waitingQueue = new WQ_WaitingQueue(waitingQueueList);


        
        addGuestButton.onClick.AddListener(() =>
        {
            if (waitingQueue.CanAddGuest())
            {
                
                if(guestList.Count > 0)
                {
                    int randomIndex = Random.Range(0, guestList.Count);
                    WQ_Guest guest = guestList[randomIndex];
                    if (guest != null)
                    {
                        guestList.RemoveAt(guestList.IndexOf(guest));

                        waitingQueue.AddGuest(guest);
                    }
                }
                
            }
        });
        
        getGuestButton.onClick.AddListener(() =>
        {

            WQ_Guest guest = waitingQueue.GetGuest();
            if (guest != null)
            {
                guest.MoveTo(new Vector3(7, 0));
            }
        });
    }


}
