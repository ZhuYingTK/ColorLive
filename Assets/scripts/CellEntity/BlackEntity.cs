using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlackEntity : CellEntity
{
    public static int cost = 1;
    public override CellData data { get; set; }
    public override void Produce()
    {
        GameManager.Instance.blackRes++;
    }

    public override void CheckStatusByTile(BoardTile tile)
    {
        var neighbors = BoardManager.Instance.Board.GetTileNeighbors(tile);
        var blackNeighbor = neighbors.Where(e => e.GetCellType() == eCellType.Black);
        int count = blackNeighbor.Count();
        
        if (status == eEntityStatus.Generating && count != 3)
        {
            StopGenerate(tile);
        }
        
        if (count == 2 || count == 3)
        {
            if(status == eEntityStatus.Dying)
                StopDie(tile);
        }
        else 
        {
            if(status == eEntityStatus.Stable) 
                StartDie(tile);
        }
    }

    public BlackEntity(eEntityStatus status = eEntityStatus.Generating)
    {
        data = new CellData()
        {
            generateTurns = GameManager.BlackTileData.generateTurns,
            productTurns = GameManager.BlackTileData.productTurns,
            dieTurns = GameManager.BlackTileData.dieTurns,
            live = GameManager.BlackTileData.live,
            type = eCellType.Black,
            viewDistance = GameManager.BlackTileData.viewDistance,
            status = status
        };
    }
    public BlackEntity(int live,eEntityStatus status = eEntityStatus.Generating)
    {
        data = new CellData()
        {
            generateTurns = GameManager.BlackTileData.generateTurns,
            productTurns = GameManager.BlackTileData.productTurns,
            dieTurns = GameManager.BlackTileData.dieTurns,
            live = live,
            type = eCellType.Black,
            viewDistance = GameManager.BlackTileData.viewDistance,
            status = status
        };
    }
}
