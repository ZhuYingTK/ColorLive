using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(BoardManager))]
public class BlockManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 显示默认Inspector内容
        base.OnInspectorGUI();

        // 获取目标脚本引用
        BoardManager myScript = (BoardManager)target;

        // 添加按钮
        if (GUILayout.Button("执行自定义方法"))
        {
            Vector2Int size = myScript.size;
            GridLayoutGroup gridLayoutGroup = myScript.GridLayoutGroup;
            myScript.content.sizeDelta =
                new Vector2(size.x * gridLayoutGroup.cellSize.x + (size.x - 1) * gridLayoutGroup.spacing.x,
                    size.y * gridLayoutGroup.cellSize.y + (size.y - 1) * gridLayoutGroup.spacing.y);
            while (myScript.content.childCount > 0)
            {
                DestroyImmediate(myScript.content.GetChild(0).gameObject);
            }

            myScript.tiles = new BoardTile[size.x, size.y];
            for (int i = 0; i < size.y; i++)
            {
                for (int j = 0; j < size.x; j++)
                {
                    var obj = (GameObject)PrefabUtility.InstantiatePrefab(myScript.BoardTilePrefab);
                    obj.transform.SetParent(myScript.content);
                    var node = obj.GetComponent<BoardTile>();
                    node.pos = new Vector2Int(j, i);
                    myScript.tiles[j, i] = node;
                }
            }
        }
    }
}
