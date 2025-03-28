using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlaceableItem
{
    public static PlaceableItem CurrentItem;
    private List<ResCostScritableObject> costSO;
    public List<sResVO> Cost { get; private set; }

    protected PlaceableItem(List<ResCostScritableObject> costSo)
    {
        costSO = costSo;
        Cost = new List<sResVO>();
        for (int i = 0; i < costSo.Count; i++)
        {
            Cost.Add(new sResVO(costSO[i].resType,costSO.Count));
        }
    }

    public void SetToCurrent()
    {
        CurrentItem = this;
    }

    public bool CheckCanPut(BoardTile target)
    {
        return false;
    }

    public void OnPutDone(BoardTile target)
    {
        if (CheckCanPut(target))
        {
            CostRes();
        }
    }

    public void CostRes()
    {
        
    }

    public void Update()
    {
        for (int i = 0; i < costSO.Count; i++)
        {
            costSO[i].Update();
            if (costSO[i].resType == Cost[i].type)
            {
                Cost[i].Count = costSO[i].Cost;
            }
            else
            {
                Debug.LogError($"发生类型不匹配");
            }
        }
    }
}

public class CellPlaceableItem : PlaceableItem
{
    public CellPlaceableItem(List<ResCostScritableObject> costSo) : base(costSo)
    {
    }
}
