using UnityEngine;
using UnityEngine.EventSystems;

public class CPW_AI : MonoBehaviour , IPointerClickHandler
{
    [SerializeField] private CPW_WindowCharacterPortrait cpw_WindowCharacterPortrait;
    
    int stat = 0;
    float moveTimer = 1f;

    public void SetUp(CPW_WindowCharacterPortrait cpw_WindowCharacterPortrait)
    {
        this.cpw_WindowCharacterPortrait = cpw_WindowCharacterPortrait;
        
    }

    private void Update()
    {
        float moveSpeed = 2f;
        switch (stat)
        {

            case 0:


                moveTimer -= Time.deltaTime;


                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                if (moveTimer <= 0f)
                {
                    stat = 1;
                    moveTimer = 1f;
                }

                break;

            case 1:


                moveTimer -= Time.deltaTime;


                transform.position += Vector3.down * moveSpeed * Time.deltaTime;
                if (moveTimer <= 0f)
                {
                    stat = 2;
                    moveTimer = 1f;
                }

                break;

            case 2:


                moveTimer -= Time.deltaTime;


                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                if (moveTimer <= 0f)
                {
                    stat = 3;
                    moveTimer = 1f;
                }

                break;

            case 3:


                moveTimer -= Time.deltaTime;


                transform.position += Vector3.up * moveSpeed * Time.deltaTime;
                if (moveTimer <= 0f)
                {
                    stat = 0;
                    moveTimer = 1f;
                }

                break;

        }


    }

    public void OnPointerClick(PointerEventData eventData)
    {
        cpw_WindowCharacterPortrait.CreateWindowPortrait(this);
    }
}
