using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackEntity : ColorEntity
{
    public static int cost = 1;
    public override ColorData data { get; set; }
    public override void OnProduce()
    {
        GameManager.Instance.blackRes++;
    }

    public BlackEntity(eEntityStatus status = eEntityStatus.Generating)
    {
        data = new ColorData()
        {
            generateTime = GameManager.blackNodeData.generateTime,
            productTime = GameManager.blackNodeData.productTime,
            dieTime = GameManager.blackNodeData.dieTime,
            live = GameManager.blackNodeData.live,
            type = eColorType.Black,
            status = status
        };
    }
    public BlackEntity(int live,eEntityStatus status = eEntityStatus.Generating)
    {
        data = new ColorData()
        {
            generateTime = GameManager.blackNodeData.generateTime,
            productTime = GameManager.blackNodeData.productTime,
            dieTime = GameManager.blackNodeData.dieTime,
            live = live,
            type = eColorType.Black,
            status = status
        };
    }
}
