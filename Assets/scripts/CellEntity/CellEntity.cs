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
    public float generateTime;
    public float productTime;
    public float dieTime;
    public int live;
}

public abstract class CellEntity : Entity
{
    public abstract CellData data { get; set; }
    private float currentGenerateTime = 0;
    private float currentproductTime = 0;
    private float currentdieTime = 0;
    public float generateProgress => currentGenerateTime / data.generateTime;
    public float productProgress => currentproductTime / data.productTime;
    public float dieProgress => currentdieTime / data.dieTime;

    public override void OnUpdate(BoardTile node, float deltaTime)
    {
        switch (data.status)
        {
            case eEntityStatus.Generating:
                AddGenerateTime(node, deltaTime);
                break;
            case eEntityStatus.Stable:
                AddProducingTime(node, deltaTime);
                break;
            case eEntityStatus.Dying:
                AddProducingTime(node, deltaTime);
                AddDyingTime(node, deltaTime);
                break;
        }
    }

    protected virtual void AddGenerateTime(BoardTile node,float deltaTime)
    {
        currentGenerateTime += deltaTime;
        if (generateProgress >= 1)
        {
            currentGenerateTime = data.generateTime;
            OnGenerate(node);
        }
        node.SetGenerateProgress(generateProgress);
    }

    protected virtual void AddDyingTime(BoardTile node,float deltaTime)
    {
        currentdieTime += deltaTime;
        if (dieProgress >= 1)
        {
            currentdieTime = data.dieTime;
            OnDied(node);
        }
        node.SetDieProgress(dieProgress);
    }
    
    protected virtual void AddProducingTime(BoardTile node,float deltaTime)
    {
        currentproductTime += deltaTime;
        if (productProgress >= 1)
        {
            currentproductTime = 0;
            OnProduce();
        }

        node.SetProduceProgress(productProgress);
    }

    public virtual void OnStopDie(BoardTile node)
    {
        data.status = eEntityStatus.Stable;
        currentdieTime = 0;
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
