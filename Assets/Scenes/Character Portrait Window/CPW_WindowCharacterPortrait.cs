using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CPW_WindowCharacterPortrait : MonoBehaviour
{

    [SerializeField] private Transform cam , followrPosition;
    [SerializeField] Button Btn_Close;

    
    private void Awake()
    {
        
        cam = transform.Find("Cam_CharacterPortrait");

        Btn_Close = transform.Find("Btn_Close").GetComponent<Button>();
        Btn_Close.onClick.AddListener(Hide);
        
        Hide();

    }

    private void Update()
    {
        cam.position = new Vector3(followrPosition.position.x, followrPosition.position.y, -10);
        

    }
    public void Show(Transform followrPosition)
    {
        Debug.Log("Show");
        gameObject.SetActive(true);
        this.followrPosition = followrPosition;
        

    }

    private void Hide()
    {
        Debug.Log("Hide");
        gameObject.SetActive(false);
    }


}
