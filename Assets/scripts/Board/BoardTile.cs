using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class BoardTile : MonoBehaviour
{
    public CellEntity cellEntity;
    public Vector2Int pos;
    private int _visibilityCount;

    public int visibilityCount
    {
        get => _visibilityCount;
        set
        {
            _visibilityCount = value;
            SetVisibilityView();
        }
    }

    public void OnUpdate(int deltaTurn)
    {
        if(cellEntity == null) return;
        cellEntity.Update(this,deltaTurn);
    }

    //点击事件
    public void OnClick()
    {
        if(_visibilityCount == 0) return;
        if (cellEntity == null || cellEntity.data.status == eEntityStatus.Generating)
        {
            if (GameManager.Instance.blackRes >= BlackEntity.cost)
            {
                cellEntity = new BlackEntity(status:eEntityStatus.Stable);
                GameManager.Instance.blackRes -= BlackEntity.cost;
                liveText.text = cellEntity.data.live.ToString();
                SetLiveText();
                SetEntityView(cellEntity.GetCellType());
                RefreshTileView();
                cellEntity.Spawned(this);
            }
        }
    }

    public void GenerateCell(eCellType type,int live)
    {
        if(cellEntity != null) return;
        switch (type)
        {
            case eCellType.Black:
                cellEntity = new BlackEntity(live,status: eEntityStatus.Generating);
                cellEntity.StartGenerate(this);
                SetLiveText();
                break;
        }
    }

    public void CheckCellEntityStatusByTile(BoardTile tile)
    {
        cellEntity?.CheckStatusByTile(tile);
    }
    

    public void EntityDestroy()
    {
        cellEntity = null;
        SetEntityView(eCellType.None);
    }

    public eCellType GetCellType()
    {
        if (cellEntity == null)
            return eCellType.None;
        else
            return cellEntity.GetCellType();
    }

    public eEntityStatus GetCellStatus()
    {
        if (cellEntity == null)
            return eEntityStatus.None;
        else
            return cellEntity.data.status;
    }

    public int? GetCellLive()
    {
        if (cellEntity == null)
            return null;
        return cellEntity.GetCellLive();
    }
}
