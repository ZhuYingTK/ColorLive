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

    public GameObject BoardTilePrefab;
    public float refreshTime = 0.1f;

    private List<BoardTile> willDieCells = new List<BoardTile>();
    
    public void Awake()
    {
        Instance = this;
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
            switch (tile.GetCellType())
            {
                case eCellType.None:
                {
                    var Neighbors = GetTileNeighbors(tile);
                    var blackEntityCount = Neighbors.Count(e => e.GetCellType() == eCellType.Black);
                    if (blackEntityCount == 3)
                    {
                        List<int> liveList = Neighbors.Where(e => e.GetCellLive() != null).Select(e => e.GetCellLive().Value)
                            .ToList();
                        int live = liveList.Min() - 1;
                        if (live > 0)
                        {
                            tile.GenerateCell(eCellType.Black,live);
                        }
                    }
                    else
                    {
                        if (tile.GetCellStatus() == eEntityStatus.Generating)
                        {
                            tile.StopGenerate();
                        }
                    }
                    break;
                }
                case eCellType.Black:
                {
                    var Neighbors = GetTileNeighbors(tile);
                    var blackEntityCount = Neighbors.Count(e => e.GetCellType() == eCellType.Black);
                    if (blackEntityCount < 2 || blackEntityCount > 3)
                    {
                        willDieCells.Add(tile);
                    }
                    else if(tile.GetCellStatus() == eEntityStatus.Dying)
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
                tileList.Add(tiles[pos.x,pos.y]);
            }
        }
        return tileList;
    }
    
}
