using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sResVO
{
    public static readonly Dictionary<eResType, sResVO> TypeDic = new Dictionary<eResType, sResVO>();
    static sResVO()
    {
        // 获取所有eResType枚举值
        foreach (eResType resType in Enum.GetValues(typeof(eResType)))
        {
            // 为每个枚举值创建新的sResVO实例并添加到字典
            TypeDic.Add(resType, new sResVO(resType,0));
        }
    }

    public static readonly Dictionary<eResType, Color> ColorDic = new Dictionary<eResType, Color>()
    {
        { eResType.Black, Color.black }
    };
    
    public eResType type;
    public int Count;

    public sResVO(eResType type, int count)
    {
        this.type = type;
        Count = count;
    }
}

public enum eResType
{
    Black,
}
