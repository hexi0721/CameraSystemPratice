using UnityEngine;

public class QAP_GameHandler : MonoBehaviour
{

    [SerializeField] QAP_WindowQuestPointer windowQuestPointer;

    private void Start()
    {
        windowQuestPointer.Show(new Vector3(5 , 4));
    }

}
