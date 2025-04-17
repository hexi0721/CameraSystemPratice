using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CPW_GameHandler : MonoBehaviour
{

    [SerializeField] private CPW_AI AI1 , AI2;
    [SerializeField] private CPW_WindowCharacterPortrait cpw_WindowCharacterPortrait;
    [SerializeField] private CPW_CharacterScriptableObject slime1 , slime2;

    [SerializeField] private CPW_Player ª±®a;



    private void Start()
    {
        //int maxExp, int maxHp, int attack, int defence
        slime1.Initialize(50 , 10 , 1 , 1);
        slime2.Initialize(100 , 20, 2, 2);

        
        AI1.SetUp(cpw_WindowCharacterPortrait , slime1);
        AI2.SetUp(cpw_WindowCharacterPortrait , slime2);


    }


    private void Update()
    {
        ª±®a.HandleManualMoveMent();
    }


}
