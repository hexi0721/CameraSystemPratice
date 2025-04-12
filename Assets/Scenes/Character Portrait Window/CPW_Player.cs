using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class CPW_Player : MonoBehaviour , IPointerClickHandler
{

    private CPW_WindowCharacterPortrait cpw_WindowCharacterPortrait;
    

    public void SetUp(CPW_WindowCharacterPortrait cpw_WindowCharacterPortrait)
    {
        this.cpw_WindowCharacterPortrait = cpw_WindowCharacterPortrait;
        
    }

    private void Update()
    {

        HandleMoveMent();


    }

    private void HandleMoveMent()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        if (v != 0 || h != 0)
        {
            float moveSpeed = 5f;
            transform.position += new Vector3(h, v) * moveSpeed * Time.deltaTime;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick!");

        cpw_WindowCharacterPortrait.Show(transform);  

    }
}
