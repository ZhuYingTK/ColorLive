using System;
using UnityEngine;

public abstract class GetResCostMethod
{
    public abstract int GetCost();
}

public enum eResCostFunctionType
{
    Exponential,//指数
    Linear,     //线性
}


[Serializable]
public class ExponentialResCostMethod : GetResCostMethod
{
    public eCellType type;
    public int k;
    public float baseNum;

    public ExponentialResCostMethod(eCellType type, int coefficientNum, float baseNum)
    {
        this.type = type;
        this.k = coefficientNum;
        this.baseNum = baseNum;
    }
    public override int GetCost()
    {
        int indexNum = BoardManager.Instance.Board.CellEntitiesTypeDic[type].Count;
        return k * (int)Mathf.Pow(baseNum, indexNum);
    }
}

[Serializable]
public class LinearResCostMethod : GetResCostMethod
{
    public eCellType type;
    public int b;
    public float k;

    public LinearResCostMethod(eCellType type, int b, float k)
    {
        this.type = type;
        this.b = b;
        this.k = k;
    }
    public override int GetCost()
    {
        int indexNum = BoardManager.Instance.Board.CellEntitiesTypeDic[type].Count;
        return (int)(k * indexNum + b);
    }
}