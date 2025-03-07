using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class BoardTile : MonoBehaviour,IClickable
{
    public CellEntity cellEntity;
    public Vector2Int pos;

    private void Update()
    {
        if (cellEntity != null)
        {
            cellEntity.OnUpdate(this, Time.deltaTime);
        }
    }

    //点击事件
    public void OnClick()
    {
        if (cellEntity == null || cellEntity.data.status == eEntityStatus.Generating)
        {
            if (GameManager.Instance.blackRes >= BlackEntity.cost)
            {
                cellEntity = new BlackEntity(status:eEntityStatus.Stable);
                GameManager.Instance.blackRes -= BlackEntity.cost;
                liveText.text = cellEntity.data.live.ToString();
                SetLiveText();
                RefreshTileView();
            }
        }
    }

    public void GenerateCell(eCellType type,int live)
    {
        if(cellEntity != null && cellEntity.data.status == eEntityStatus.Generating) return;
        switch (type)
        {
            case eCellType.Black:
                cellEntity = new BlackEntity(live,status: eEntityStatus.Generating);
                cellEntity.OnStartGenerate(this);
                SetLiveText();
                break;
        }
    }

    public void StartDie()
    {
        cellEntity.OnStartDie(this);
    }

    public void StopDie()
    {
        cellEntity.OnStopDie(this);
    }

    public void StopGenerate()
    {
        cellEntity.OnStopGenerate(this);
    }

    public void EntityDead()
    {
        cellEntity = null;
        SetBody(eCellType.None);
        RefreshTileView();
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
