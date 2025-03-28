using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "创建细胞数据")]
public class CellDataScriptableObject : ScriptableObject
{
    public eCellType type;
    public int generateTurns;
    public int productTurns;
    public int dieTurns;
    public int live;
    public int viewDistance;
}
