using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CPW_AI : MonoBehaviour , IPointerClickHandler
{
    [SerializeField] private CPW_WindowCharacterPortrait cpw_WindowCharacterPortrait;
    CPW_CharacterScriptableObject cpw_CharacterScriptableObject;
    int stat = 0;
    float moveTimer = 1f;

    float addExpTimer;
    float addExpMaxTimer = .025f;

    public void SetUp(CPW_WindowCharacterPortrait cpw_WindowCharacterPortrait , CPW_CharacterScriptableObject cpw_CharacterScriptableObject)
    {
        this.cpw_WindowCharacterPortrait = cpw_WindowCharacterPortrait;
        this.cpw_CharacterScriptableObject = cpw_CharacterScriptableObject;

        cpw_CharacterScriptableObject.TF = transform;

        addExpTimer = addExpMaxTimer;
    }


    private void Update()
    {
        HandleMovement();
        addExpTimer -= Time.deltaTime;
        if (addExpTimer <= 0f)
        {
            addExpTimer += addExpMaxTimer;
            cpw_CharacterScriptableObject.AddExp(1);

            
        }

    }

    private void HandleMovement()
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
        cpw_WindowCharacterPortrait.CreateWindowPortrait(cpw_CharacterScriptableObject);
    }
}
