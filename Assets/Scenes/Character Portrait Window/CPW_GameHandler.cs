using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CPW_GameHandler : MonoBehaviour
{

    [SerializeField] private Transform player;
    [SerializeField] private CPW_WindowCharacterPortrait cpw_WindowCharacterPortrait;

    

    private void Start()
    {

        CPW_Player cpw_Player = player.GetComponent<CPW_Player>();
        cpw_Player.SetUp(cpw_WindowCharacterPortrait);


    }





}
