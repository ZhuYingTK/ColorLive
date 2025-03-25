using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;
    public GameObject BoardTilePrefab;
    public Transform BoardGameObject;
    public Board Board => board;
    private Board board;
    private long _currentTurn;
    private float updateTime = 1;
    public bool stopUpdate = false;
    private float currentTime;
    public void Awake()
    {
        _currentTurn = 0;
        Instance = this;
        board = new Board();
    }

    public void Update()
    {
        if (!stopUpdate)
        {
            currentTime += Time.deltaTime;
            if (currentTime > updateTime)
            {
                currentTime = 0;
                board.Update(1);
                _currentTurn++;
            }
        }
    }
    
    public void RefreshChunk(Rect rect)
    {
        board.SetCameraChunkActivity(rect);
    }
    
    
}
