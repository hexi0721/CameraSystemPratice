using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class CPW_Player : MonoBehaviour
{



    public void HandleManualMoveMent()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        if (v != 0 || h != 0)
        {
            float moveSpeed = 5f;
            transform.position += new Vector3(h, v) * moveSpeed * Time.deltaTime;
        }
    }


}
