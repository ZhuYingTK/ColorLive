using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Board
{
    
    private Dictionary<int, BoardTile> _tiles = new Dictionary<int, BoardTile>(capacity:100000);
    public IEnumerable<BoardTile> TileList => _tiles.Values;

    public readonly Dictionary<eCellType, List<CellEntity>> CellEntitiesTypeDic =
        new Dictionary<eCellType, List<CellEntity>>();

    public Board()
    {
        for (int i = 0; i < Chunk.MapChunkSize; i++)
        {
            _chunks[i] = new Chunk[Chunk.MapChunkSize];
        }

        foreach (eCellType cellType in Enum.GetValues(typeof(eCellType)))
        {
            // 为每个枚举值创建新的sResVO实例并添加到字典
            CellEntitiesTypeDic.Add(cellType, new List<CellEntity>());
        }
    }

    public void Update(int turn)
    {
        foreach (var tile in TileList)
        {
            if (tile.GetCellType() == eCellType.None)
            {
                var list = GetTileNeighbors(tile);
                var blackNeighbor = list.Where(e => e.GetCellType() == eCellType.Black);
                BoardTile[] boardTiles = blackNeighbor as BoardTile[] ?? blackNeighbor.ToArray();
                if (boardTiles.Length == 3)
                {
                    int live = boardTiles.Min(e => (int)e.GetCellLive()) - 1;
                    if(live > 0) 
                        tile.GenerateCell(eCellType.Black,live);
                }
            }
        }

        foreach (var tile in TileList)
        {
            tile.CheckCellEntityStatusByTile(tile);
        }
        
        foreach (var tile in TileList)
        {
            tile.OnUpdate(turn);
        }
    }
    
    /// <summary>
    /// 添加单片
    /// </summary>
    /// <param name="tile"></param>
    public void AddTile(BoardTile tile)
    {
        this[tile.pos] = tile;
    }

    public List<BoardTile> GetTilesInDistance(Vector2Int pos, int distance)
    {
        List<BoardTile> results = new List<BoardTile>();
        for (int dx = -distance; dx <= distance; dx++)
        {
            int remaining = distance - Mathf.Abs(dx);
            
            // 遍历对应x偏移量下的y偏移量范围
            for (int dy = -remaining; dy <= remaining; dy++)
            {
                if(dx == 0 && dy == 0) continue;
                int x = pos.x + dx;
                int y = pos.y + dy;
                if(this[x, y] == null)
                    LoadChunkByTilePos(new Vector2Int(x, y));
                results.Add(this[x, y]);
            }
        }
        return results;
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
                if (this[pos.x, pos.y] != null)
                {
                    tileList.Add(this[pos.x, pos.y]);
                }
            }
        }
        return tileList;
    }
    
    public void AddCellEntity(CellEntity entity)
    {
        CellEntitiesTypeDic[entity.GetCellType()].Add(entity);
    }

    public void RemoveCellEntity(CellEntity entity)
    {
        CellEntitiesTypeDic[entity.GetCellType()].Remove(entity);
    }

    
    #region 地块生成

    public static BoardTile GenerateBoardTile(int x, int y)
    {
        return GenerateBoardTile(new Vector2Int(x, y));
    }
    // ReSharper disable Unity.PerformanceAnalysis
    public static BoardTile GenerateBoardTile(Vector2Int pos)
    {
        var obj = GameObject.Instantiate(BoardManager.Instance.BoardTilePrefab, BoardManager.Instance.BoardGameObject);
        var tile = obj.GetComponent<BoardTile>();
        tile.pos = pos;
        obj.transform.position = new Vector3(pos.x, pos.y, 0);
        tile.visibilityCount = IsOriginVisibleTile(pos) ? 1 : 0;
        return tile;
    }

    public static bool IsOriginVisibleTile(Vector2Int pos)
    {
        return pos.x is >= -1 and <= 1 && pos.y is >= -1 and <= 1;
    }

    #endregion

    #region 坐标部分

    public BoardTile this[Vector2Int pos]
    {
        get => _tiles.GetValueOrDefault(PosToID(pos));
        set => _tiles[PosToID(pos)] = value;
    }

    public BoardTile this[int row, int col]
    {
        get => _tiles.GetValueOrDefault(PosToID(row, col));
        set => _tiles[PosToID(row, col)] = value;
    }
    
    public static int PosToID(Vector2Int pos)
    {
        short x = (short)pos.x;
        short y = (short)pos.y;
        return (x << 16) | (y & 0xFFFF);
    }
    
    public static int PosToID(int X,int Y)
    {
        short x = (short)X;
        short y = (short)Y;
        return (x << 16) | (y & 0xFFFF);
    }

    public static Vector2Int IDToPos(int id)
    {
        short x = (short)(id >> 16); // 提取高 16 位
        short y = (short)(id & 0xFFFF); // 提取低 16 位
        return new Vector2Int(x, y);
    }

    #endregion
    

}

