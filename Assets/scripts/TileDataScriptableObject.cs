using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "创建细胞数据")]
public class TileDataScriptableObject : ScriptableObject
{
    public eCellType type;
    public float generateTime;
    public float productTime;
    public float dieTime;
    public int live;
}
