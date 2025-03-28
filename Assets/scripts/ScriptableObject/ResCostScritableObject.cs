using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "创建消耗数据")]
public class ResCostScritableObject : ScriptableObject
{
    public eResType resType;
    public eResCostFunctionType functionType;
    [SerializeReference]public GetResCostMethod method;
    public int Cost {get; private set; }
    
    public void Update()
    {
        Cost = method.GetCost();
    }
}
