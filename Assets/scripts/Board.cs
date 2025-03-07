using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board
{
    private Dictionary<int, BoardTile> _tiles = new Dictionary<int, BoardTile>(capacity:100000);
    public IEnumerable<BoardTile> TileList => _tiles.Values;
    
    

    public void AddTile(BoardTile tile)
    {
        
    }
    
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
}

