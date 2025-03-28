using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum eCellType
{
    None,
    Black,
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
    public int viewDistance;
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
    public eEntityStatus status => data.status;

    public override void Update(BoardTile node, int deltaTurn)
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

    public virtual void CheckStatusByTile(BoardTile tile)
    {
    }

    protected virtual void AddGenerateTurns(BoardTile node,int deltaTurn)
    {
        currentGenerateTurns += deltaTurn;
        if (generateProgress >= 1)
        {
            currentGenerateTurns = data.generateTurns;
            Generate(node);
        }
        node.SetGenerateProgress(generateProgress);
    }

    protected virtual void AddDyingTurns(BoardTile node,int deltaTurn)
    {
        currentdieTurns += deltaTurn;
        if (dieProgress >= 1)
        {
            currentdieTurns = data.dieTurns;
            Died(node);
        }
        node.SetDieProgress(dieProgress);
    }
    
    protected virtual void AddProducingTurns(BoardTile node,int deltaTurn)
    {
        currentproductTurns += deltaTurn;
        if (productProgress >= 1)
        {
            currentproductTurns = 0;
            Produce();
        }

        node.SetProduceProgress(productProgress);
    }

    public virtual void StopDie(BoardTile node)
    {
        data.status = eEntityStatus.Stable;
        currentdieTurns = 0;
        node.SetDieProgress(dieProgress);
    }
    
    public virtual void StartGenerate(BoardTile node)
    {
        data.status = eEntityStatus.Generating;
    }

    public virtual void StopGenerate(BoardTile node)
    {
        data.status = eEntityStatus.None;
        node.EntityDestroy();
    }

    public virtual void Generate(BoardTile node)
    {
        data.status = eEntityStatus.Stable;
        node.SetEntityView(GetCellType());
        Spawned(node);
    }

    public virtual void Spawned(BoardTile node)
    {
        var board = BoardManager.Instance.Board;
        board.AddCellEntity(this);
        var target = board.GetTilesInDistance(node.pos,data.viewDistance);
        for (int i = 0; i < target.Count; i++)
        {
            target[i].visibilityCount++;
        }
    }

    public virtual void StartDie(BoardTile node)
    {
        data.status = eEntityStatus.Dying;
    }

    public virtual void Died(BoardTile node)
    {
        node.EntityDestroy();
        var board = BoardManager.Instance.Board;
        board.RemoveCellEntity(this);
        var target = board.GetTilesInDistance(node.pos,data.viewDistance);
        for (int i = 0; i < target.Count; i++)
        {
            target[i].visibilityCount--;
        }
    }

    public abstract void Produce();

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
