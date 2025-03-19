using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Board
{
    
    private Dictionary<int, BoardTile> _tiles = new Dictionary<int, BoardTile>(capacity:100000);
    public IEnumerable<BoardTile> TileList => _tiles.Values;

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

    #region 坐标部分

    public BoardTile this[Vector2Int pos]
    {
        get => _tiles[PosToID(pos)];
        set => _tiles[PosToID(pos)] = value;
    }

    public BoardTile this[int row, int col]
    {
        get => _tiles[PosToID(row, col)];
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

