using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackEntity : ColorEntity
{
    public static int cost;
    public override ColorData data { get; set; }
    public override void OnProduce()
    {
        GameManager.Instance.blackRes++;
    }

    public BlackEntity(eEntityStatus status = eEntityStatus.Generating,int live = 5)
    {
        data = new ColorData()
        {
            generateTime = 3,
            productTime = 2,
            dieTime = 4,
            live = live,
            type = eColorType.Black,
            status = status
        };
    }
}
