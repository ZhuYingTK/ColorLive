
using UnityEngine;

public class Chunk
{
    public const int ChunkSize = 16;
    public const int MapChunkSize = 32;
    private static Vector2Int chunkOffset => new Vector2Int(MapChunkSize / 2, MapChunkSize / 2);
    public Vector2Int pos;
    
    public static Vector2Int WorldPosToChunkPos(Vector2 worldPos)
    {
        return TilePosToChunkPos(new Vector2Int((int)worldPos.x,(int)worldPos.y));
    }

    public static Vector2Int TilePosToChunkPos(Vector2Int tilePos)
    {
        int x = FloorDivision(tilePos.x, ChunkSize);
        int y = FloorDivision(tilePos.y, ChunkSize);
        return new Vector2Int(x,y) + chunkOffset;
    }
    
    static int FloorDivision(int dividend, int divisor)
    {
        int quotient = dividend / divisor;
        int remainder = dividend % divisor;

        // 如果余数不为零且结果应为负数，则向下取整
        quotient -= ((remainder != 0) & ((dividend < 0) ^ (divisor < 0))) ? 1 : 0;

        return quotient;
    }

    public Vector2Int ChunkOriginPos => (pos - chunkOffset) * ChunkSize;
}
