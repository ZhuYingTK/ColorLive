using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackEntity : ColorEntity
{
    public static int cost = 1;
    public override CellData data { get; set; }
    public override void OnProduce()
    {
        GameManager.Instance.blackRes++;
    }

    public BlackEntity(eEntityStatus status = eEntityStatus.Generating)
    {
        data = new CellData()
        {
            generateTime = GameManager.BlackTileData.generateTime,
            productTime = GameManager.BlackTileData.productTime,
            dieTime = GameManager.BlackTileData.dieTime,
            live = GameManager.BlackTileData.live,
            type = eCellType.Black,
            status = status
        };
    }
    public BlackEntity(int live,eEntityStatus status = eEntityStatus.Generating)
    {
        data = new CellData()
        {
            generateTime = GameManager.BlackTileData.generateTime,
            productTime = GameManager.BlackTileData.productTime,
            dieTime = GameManager.BlackTileData.dieTime,
            live = live,
            type = eCellType.Black,
            status = status
        };
    }
}
