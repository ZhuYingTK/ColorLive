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
    public void Awake()
    {
        Instance = this;
        board = new Board();
    }

    public void Start()
    {
    }

    public void RefreshChunk(Rect rect)
    {
        board.SetChunkActivity(rect);
    }
    
    
}
