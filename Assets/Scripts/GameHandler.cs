using System;
using Unity.Cinemachine;
using UnityEngine;

public class GameHandler : MonoBehaviour
{

    [SerializeField] Transform cineMachineTarget;
    [SerializeField] PlayerCamera virtualCam;
    [SerializeField] float zoom = 5f;

    private void Start()
    {
        virtualCam.SetGetCameraZoomFunc(() => zoom);
    }

    private void Update()
    {

        CineMachineTarget();
          
        HandleManualZoom();

        

        
        


    }

    private void HandleManualZoom()
    {
        float zoomSpeed = 10f;
        if(Input.GetKey(KeyCode.KeypadPlus))
        {
            zoom -= Time.deltaTime * zoomSpeed;
        }
        if (Input.GetKey(KeyCode.KeypadMinus))
        {
            zoom += Time.deltaTime * zoomSpeed;
        }

        float zoomScrollSpeed = 50f;
        if (Input.mouseScrollDelta.y > 0)
        {
            zoom -= Time.deltaTime * zoomScrollSpeed;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            zoom += Time.deltaTime * zoomScrollSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CinematicBar.ShowBarStatic(200, 2f);

            zoom -= Time.deltaTime * zoomScrollSpeed;
            virtualCam.GetCameraZoom(zoom);

        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            CinematicBar.HideBarStatic(2f);

            zoom += Time.deltaTime * zoomScrollSpeed;
            virtualCam.GetCameraZoom(zoom);
        }

        zoom = Mathf.Clamp(zoom, 5f, 10f);
    }

    public void CineMachineTarget()
    {
        cineMachineTarget.transform.position = Utils.GetMouseWorldPosZeroZ();
    }

}
