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
    
    public BoardTile[,] tiles;

    public GridLayoutGroup GridLayoutGroup;
    public GameObject BoardTilePrefab;
    public RectTransform content;
    public Vector2Int size;
    public int width => size.x;
    public int height => size.y;
    public float refreshTime = 0.1f;

    private List<BoardTile> willDieCells = new List<BoardTile>();
    
    public void Awake()
    {
        Instance = this;
        Init();
    }

    private void Init()
    {
        tiles = new BoardTile[width, height];
        for (int i = 0; i < content.transform.childCount; i++)
        {
            BoardTile tile = content.transform.GetChild(i).GetComponent<BoardTile>();
            tiles[tile.pos.x, tile.pos.y] = tile;
        }
    }

    public void Update()
    {
        RefreshTiles();
    }

    public void RefreshTiles()
    {
        willDieCells.Clear();
        foreach (BoardTile tile in tiles)
        {
            switch (tile.GetColorType())
            {
                case eCellType.None:
                {
                    var Neighbors = GetTileNeighbors(tile);
                    var blackEntityCount = Neighbors.Count(e => e.GetColorType() == eCellType.Black);
                    if (blackEntityCount == 3)
                    {
                        List<int> liveList = Neighbors.Where(e => e.GetColorLive() != null).Select(e => e.GetColorLive().Value)
                            .ToList();
                        int live = liveList.Min() - 1;
                        if (live > 0)
                        {
                            tile.Generate(eCellType.Black,live);
                        }
                    }
                    else
                    {
                        if (tile.GetColorStatus() == eEntityStatus.Generating)
                        {
                            tile.StopGenerate();
                        }
                    }
                    break;
                }
                case eCellType.Black:
                {
                    var Neighbors = GetTileNeighbors(tile);
                    var blackEntityCount = Neighbors.Count(e => e.GetColorType() == eCellType.Black);
                    if (blackEntityCount < 2 || blackEntityCount > 3)
                    {
                        willDieCells.Add(tile);
                    }
                    else if(tile.GetColorStatus() == eEntityStatus.Dying)
                    {
                        tile.StopDie();
                    }
                    break;
                }
            }
        }

        for (int i = 0; i < willDieCells.Count; i++)
        {
            willDieCells[i].StartDie();
        }
    }

    public List<BoardTile> GetTileNeighbors(BoardTile tile)
    {
        List<BoardTile> tileList = new List<BoardTile>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;
                var pos = tile.pos + new Vector2Int(i, j);
                if (PosInRange(pos))
                {
                    tileList.Add(tiles[pos.x,pos.y]);
                }
            }
        }
        return tileList;
    }

    public bool PosInRange(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
    }
}
