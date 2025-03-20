using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum eCellType
{
    None,
    Black
}

public enum eEntityStatus
{
    None,
    Generating,
    Stable,
    Dying
}

public class CellData
{
    public eCellType type;
    public eEntityStatus status;
    public int generateTurns;
    public int productTurns;
    public int dieTurns;
    public int live;
}

public abstract class CellEntity : Entity
{
    public abstract CellData data { get; set; }
    private int currentGenerateTurns = 0;
    private int currentproductTurns = 0;
    private int currentdieTurns = 0;
    public float generateProgress => (float)currentGenerateTurns / data.generateTurns;
    public float productProgress => (float)currentproductTurns / data.productTurns;
    public float dieProgress => (float)currentdieTurns / data.dieTurns;

    public override void OnUpdate(BoardTile node, int deltaTurn)
    {
        switch (data.status)
        {
            case eEntityStatus.Generating:
                AddGenerateTurns(node, deltaTurn);
                break;
            case eEntityStatus.Stable:
                AddProducingTurns(node, deltaTurn);
                break;
            case eEntityStatus.Dying:
                AddProducingTurns(node, deltaTurn);
                AddDyingTurns(node, deltaTurn);
                break;
        }
    }

    protected virtual void AddGenerateTurns(BoardTile node,int deltaTurn)
    {
        currentGenerateTurns += deltaTurn;
        if (generateProgress >= 1)
        {
            currentGenerateTurns = data.generateTurns;
            OnGenerate(node);
        }
        node.SetGenerateProgress(generateProgress);
    }

    protected virtual void AddDyingTurns(BoardTile node,int deltaTurn)
    {
        currentdieTurns += deltaTurn;
        if (dieProgress >= 1)
        {
            currentdieTurns = data.dieTurns;
            OnDied(node);
        }
        node.SetDieProgress(dieProgress);
    }
    
    protected virtual void AddProducingTurns(BoardTile node,int deltaTurn)
    {
        currentproductTurns += deltaTurn;
        if (productProgress >= 1)
        {
            currentproductTurns = 0;
            OnProduce();
        }

        node.SetProduceProgress(productProgress);
    }

    public virtual void OnStopDie(BoardTile node)
    {
        data.status = eEntityStatus.Stable;
        currentdieTurns = 0;
        node.SetDieProgress(dieProgress);
    }
    
    public virtual void OnStartGenerate(BoardTile node)
    {
        data.status = eEntityStatus.Generating;
    }

    public virtual void OnStopGenerate(BoardTile node)
    {
        node.EntityDead();
    }

    public virtual void OnGenerate(BoardTile node)
    {
        data.status = eEntityStatus.Stable;
        node.SetBody(GetCellType());
    }

    public virtual void OnStartDie(BoardTile node)
    {
        data.status = eEntityStatus.Dying;
    }

    public virtual void OnDied(BoardTile node)
    {
        node.EntityDead();
    }

    public abstract void OnProduce();

    public eCellType GetCellType()
    {
        if (data.status == eEntityStatus.Generating) 
            return eCellType.None;
        return data.type;
    }

    public virtual int? GetCellLive()
    {
        return data.status == eEntityStatus.Generating ? null : data.live;
    }
}
