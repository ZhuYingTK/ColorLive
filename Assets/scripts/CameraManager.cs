using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class CameraManager : MonoBehaviour
{
    public Camera mainCamera;
    private Vector3 dragOriginScreen; // 记录初始屏幕坐标
    private Vector3 dragOriginWorld; // 记录初始相机位置

    private bool isDragging;
    private bool isPointDown;
    private float dragThreshold = 5;

    private void OnEnable()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        OnCameraMoving();
    }

    void Update()
    {
        //暂停
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BoardManager.Instance.stopUpdate = !BoardManager.Instance.stopUpdate;
        }
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
        {
            // 记录初始屏幕坐标和相机位置
            dragOriginScreen = Input.mousePosition;
            dragOriginWorld = mainCamera.transform.position;
            isDragging = false;
            isPointDown = true;
        }

        if (Input.GetMouseButton(0) && isPointDown)
        {
            // 计算鼠标位移（屏幕空间）
            Vector3 deltaScreen = Input.mousePosition - dragOriginScreen;
            if (deltaScreen.magnitude > dragThreshold)
            {
                isDragging = true;
                // 将屏幕位移转换为世界位移
                Vector3 deltaWorld = mainCamera.ScreenToWorldPoint(deltaScreen) - mainCamera.ScreenToWorldPoint(Vector3.zero);
                // 直接应用位移到初始相机位置（反向移动）
                mainCamera.transform.position = dragOriginWorld - deltaWorld;
                OnCameraMoving();
            }

        }

        if (Input.GetMouseButtonUp(0) && isPointDown)
        {
            if (!isDragging)
            {
                OnClick();
            }
            else
            {
                OnCameraMoveDown();
            }

            isPointDown = false;
        }
    }

    private void OnCameraMoving()
    {
        var rect = GetOrthographicCameraRect(Camera.main);
        BoardManager.Instance.RefreshChunk(rect);
    }

    private void OnCameraMoveDown()
    {
        
    }

    private void OnClick()
    {
        Collider2D[] col = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        foreach (var result in col)
        {
            if (result.gameObject.TryGetComponent<BoardTile>(out var tile))
            {
                tile.OnClick();
                return;
            }
        }
    }
    
    // 检测是否点击到UI元素（需引用 UnityEngine.EventSystems）
    private bool IsPointerOverUI()
    {
        // 获取当前指针下的所有 UI 元素
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        // 遍历所有结果，检查是否有 Tag 为 MapUI 的 UI 元素
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.layer.Equals("UI"))
            {
                return true;
            }
        }
        return false;
    }
    
    /// <summary>
    /// 获得相机对应场景内矩形
    /// </summary>
    /// <param name="camera"></param>
    /// <returns></returns>
    public static Rect GetOrthographicCameraRect(Camera camera)
    {

        // 获取相机的正交尺寸
        float orthographicSize = camera.orthographicSize;

        // 获取相机的宽高比
        float aspectRatio = camera.aspect;

        // 计算相机视口的宽度和高度
        float width = orthographicSize * aspectRatio * 2;
        float height = orthographicSize * 2;

        // 获取相机的位置
        Vector3 cameraPosition = camera.transform.position;

        // 计算相机视口的四个角
        Vector3 bottomLeft = cameraPosition - new Vector3(width / 2, height / 2, 0);

        // 创建一个 Rect 来表示相机视口
        Rect cameraRect = new Rect(bottomLeft.x, bottomLeft.y, width, height);

        return cameraRect;
    }
}