using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Board
{
    
    private Dictionary<int, BoardTile> _tiles = new Dictionary<int, BoardTile>(capacity:100000);
    public IEnumerable<BoardTile> TileList => _tiles.Values;

    public Board()
    {
        for (int i = 0; i < Chunk.MapChunkSize; i++)
        {
            _chunks[i] = new Chunk[Chunk.MapChunkSize];
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
                if (blackNeighbor.Count() == 3)
                {
                    int live = blackNeighbor.Min(e => (int)e.GetCellLive()) - 1;
                    if(live > 0) 
                        tile.GenerateCell(eCellType.Black,live);
                }
            }
        }

        foreach (var tile in TileList)
        {
            if (tile.GetCellType() == eCellType.Black)
            {
                var list = GetTileNeighbors(tile);
                var blackNeighbor = list.Where(e => e.GetCellType() == eCellType.Black);
                int count = blackNeighbor.Count();
                if (count != 2 && count != 3)
                {
                    tile.StartDie();
                }
            }
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
    
    #region 地块生成

    public static BoardTile GenerateBoardTile(int x, int y)
    {
        return GenerateBoardTile(new Vector2Int(x, y));
    }
    public static BoardTile GenerateBoardTile(Vector2Int pos)
    {
        var obj = GameObject.Instantiate(BoardManager.Instance.BoardTilePrefab, BoardManager.Instance.BoardGameObject);
        var tile = obj.GetComponent<BoardTile>();
        tile.pos = pos;
        obj.transform.position = new Vector3(pos.x, pos.y, 0);
        return tile;
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

