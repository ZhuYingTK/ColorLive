using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum eColorType
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

public class ColorData
{
    public eColorType type;
    public eEntityStatus status;
    public float generateTime;
    public float productTime;
    public float dieTime;
    public int live;
}

public abstract class ColorEntity
{
    public abstract ColorData data { get; set; }
    private float currentGenerateTime = 0;
    private float currentproductTime = 0;
    private float currentdieTime = 0;
    public float generateProgress => currentGenerateTime / data.generateTime;
    public float productProgress => currentproductTime / data.productTime;
    public float dieProgress => currentdieTime / data.dieTime;

    public virtual void OnUpdate(BlockNode node, float deltaTime)
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

    protected virtual void AddGenerateTime(BlockNode node,float deltaTime)
    {
        currentGenerateTime += deltaTime;
        if (generateProgress >= 1)
        {
            currentGenerateTime = data.generateTime;
            OnGenerate(node);
        }
        node.SetGenerateProgress(generateProgress);
    }

    protected virtual void AddDyingTime(BlockNode node,float deltaTime)
    {
        currentdieTime += deltaTime;
        if (dieProgress >= 1)
        {
            currentdieTime = data.dieTime;
            OnDied(node);
        }
        node.SetDieProgress(dieProgress);
    }
    
    protected virtual void AddProducingTime(BlockNode node,float deltaTime)
    {
        currentproductTime += deltaTime;
        if (productProgress >= 1)
        {
            currentproductTime = 0;
            OnProduce();
        }

        node.SetProduceProgress(productProgress);
    }

    public virtual void OnStopDie(BlockNode node)
    {
        data.status = eEntityStatus.Stable;
        currentdieTime = 0;
        node.SetDieProgress(dieProgress);
    }
    
    public virtual void OnStartGenerate(BlockNode node)
    {
        data.status = eEntityStatus.Generating;
    }

    public virtual void OnStopGenerate(BlockNode node)
    {
        node.EntityDead();
    }

    public virtual void OnGenerate(BlockNode node)
    {
        data.status = eEntityStatus.Stable;
        node.SetBody(GetColorType());
    }

    public virtual void OnStartDie(BlockNode node)
    {
        data.status = eEntityStatus.Dying;
    }

    public virtual void OnDied(BlockNode node)
    {
        node.EntityDead();
    }

    public abstract void OnProduce();

    public eColorType GetColorType()
    {
        if (data.status == eEntityStatus.Generating) 
            return eColorType.None;
        return data.type;
    }
}
