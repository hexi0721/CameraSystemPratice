using System;
using UnityEngine;
using Unity.Cinemachine;

public class PlayerCamera : MonoBehaviour
{
    Func<Vector3> GetFollowPositionFunc;
    Func<float> GetCameraZoomFunc;

    [SerializeField] int fps;
    CinemachineCamera cam;

    private void Start()
    {
        cam = GetComponent<CinemachineCamera>();
        
    }

    public void SetUp(Func<Vector3> GetFollowPositionFunc , Func<float> GetCameraZoomFunc)
    {
        this.GetFollowPositionFunc = GetFollowPositionFunc;
        this.GetCameraZoomFunc = GetCameraZoomFunc;
    }

    public void GetCameraZoom(float zoom)
    {
        SetGetCameraZoomFunc(() => zoom);
    }

    public void SetGetCameraZoomFunc(Func<float> GetCameraZoomFunc)
    {
        this.GetCameraZoomFunc = GetCameraZoomFunc;
    }

    public void GetNewCameraFollow(Vector3 position)
    {
        SetGetNewCameraFollowFunc(() => position);
    }

    public void SetGetNewCameraFollowFunc(Func<Vector3> GetFollowPositionFunc)
    {
        this.GetFollowPositionFunc = GetFollowPositionFunc;
    }

    private void Update()
    {
        // HandleMoveMent();
        HandleZoom();

        

    }

    private void HandleZoom()
    {
        float cameraZoom = GetCameraZoomFunc();

        float diffCameraZoom = cameraZoom - cam.Lens.OrthographicSize;
        float camaraZoomSpeed = 5f;

        cam.Lens.OrthographicSize += diffCameraZoom * camaraZoomSpeed * Time.deltaTime;

        if (diffCameraZoom > 0)
        {
            if(cam.Lens.OrthographicSize > cameraZoom)
            {
                cam.Lens.OrthographicSize = cameraZoom;
            }

        }
        else  // 從遠到近時，diffCameraZoom 會是負值，當FPS過小，cam.orthographicSize += diffCameraZoom * camaraZoomSpeed * Time.deltaTime 做完，cam.orthographicSize 最終會小於cameraZoom = 5，所以把cameraZoom 給 cam.orthographicSize;
        {
            if (cam.Lens.OrthographicSize < cameraZoom)
            {
                cam.Lens.OrthographicSize = cameraZoom;
            }
            
        }
    }

    private void HandleMoveMent()
    {
        Application.targetFrameRate = fps; // 當fps越小   Time.deltaTime 越大

        Vector3 cameraFollowPosition = GetFollowPositionFunc();
        cameraFollowPosition.z = transform.position.z;

        Vector3 cameraMoveDir = (cameraFollowPosition - transform.position).normalized;
        float distance = Vector3.Distance(cameraFollowPosition, transform.position);
        float cameraMoveSpeed = 1f;


        if (distance > 0)
        {
            Vector3 newCameraPostion = transform.position + cameraMoveDir * distance * cameraMoveSpeed * Time.deltaTime;
            float newDistance = Vector3.Distance(newCameraPostion, cameraFollowPosition);

            if (newDistance > distance) // 如果下一幀移動到的鏡頭位置離目標位置  遠於  先前當前幀的鏡頭位置到目標位置 就不必使用下一幀的鏡頭位置 直接使用目標位置
            {
                newCameraPostion = cameraFollowPosition;
            }


            transform.position = newCameraPostion;
        }
    }


}
