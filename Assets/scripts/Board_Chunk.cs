using System.Collections.Generic;
using UnityEngine;

public partial class Board
{

    private Chunk[,] _chunks = new Chunk[Chunk.MapChunkSize, Chunk.MapChunkSize];

    public void SetChunkActivity(Rect rect)
    {
        Vector2Int startPos = Chunk.WorldPosToChunkPos(rect.min);
        Vector2Int endPos = Chunk.WorldPosToChunkPos(rect.max);
        for(int i = startPos.x;i <= endPos.x;i++)
        {
            for (int j = startPos.y; j <= endPos.y; j++)
            {
                LoadChunkByChunkPos(new Vector2Int(i, j));
            }
        }
    }
    
    public void LoadChunkByTilePos(Vector2Int tilePos)
    {
        Vector2Int pos = Chunk.TilePosToChunkPos(tilePos);
        LoadChunkByChunkPos(pos);
    }
    public void LoadChunkByChunkPos(Vector2Int chunkPos)
    {
        if(!CheckChunkPos(chunkPos))return;
        if(_chunks[chunkPos.x,chunkPos.y] != null) return;
        Chunk chunk = new Chunk();
        _chunks[chunkPos.x, chunkPos.y] = chunk;
        chunk.pos = chunkPos;
        int offset_X = chunk.ChunkOriginPos.x;
        int offset_Y = chunk.ChunkOriginPos.y;
        for (int i = 0; i < Chunk.ChunkSize; i++)
        {
            for (int j = 0; j < Chunk.ChunkSize; j++)
            {
                var tile = GenerateBoardTile(i + offset_X, j + offset_Y);
                AddTile(tile);
            }
        }
    }

    public bool CheckChunkPos(Vector2Int pos)
    {
        return pos.x is >= 0 and < Chunk.MapChunkSize && pos.y is >= 0 and < Chunk.MapChunkSize;
    }
}