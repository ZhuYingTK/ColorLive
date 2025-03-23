using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackEntity : CellEntity
{
    public static int cost = 1;
    public override CellData data { get; set; }
    public override void Produce()
    {
        GameManager.Instance.blackRes++;
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
