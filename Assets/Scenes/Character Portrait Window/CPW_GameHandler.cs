using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CPW_GameHandler : MonoBehaviour
{

    [SerializeField] private CPW_AI AI1 , AI2;
    [SerializeField] private CPW_WindowCharacterPortrait cpw_WindowCharacterPortrait;
    [SerializeField] private CPW_Player ª±®a;


    private void Start()
    {


        AI1.SetUp(cpw_WindowCharacterPortrait);
        AI2.SetUp(cpw_WindowCharacterPortrait);


    }


    private void Update()
    {
        ª±®a.HandleManualMoveMent();
    }


}
