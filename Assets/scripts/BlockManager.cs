using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class BlockManager : MonoBehaviour
{
    public static BlockManager Instance;
    
    public BlockNode[,] nodes;

    public GridLayoutGroup GridLayoutGroup;
    public GameObject NodePrefab;
    public RectTransform content;
    public Vector2Int size;
    public int width => size.x;
    public int height => size.y;
    public float refreshTime = 0.1f;

    private List<BlockNode> willDieNodes = new List<BlockNode>();
    
    public void Awake()
    {
        Instance = this;
        Init();
    }

    private void Init()
    {
        nodes = new BlockNode[width, height];
        for (int i = 0; i < content.transform.childCount; i++)
        {
            BlockNode node = content.transform.GetChild(i).GetComponent<BlockNode>();
            nodes[node.pos.x, node.pos.y] = node;
        }
    }

    public void Update()
    {
        RefreshNodes();
    }

    public void RefreshNodes()
    {
        willDieNodes.Clear();
        foreach (BlockNode node in nodes)
        {
            switch (node.GetColorType())
            {
                case eColorType.None:
                {
                    var Neighbors = GetNodeNeighbors(node);
                    var blackEntityCount = Neighbors.Count(node => node.GetColorType() == eColorType.Black);
                    if (blackEntityCount == 3)
                    {
                        List<int> liveList = Neighbors.Where(node => node.GetColorLive() != null).Select(node => node.GetColorLive().Value)
                            .ToList();
                        int live = liveList.Min() - 1;
                        if (live > 0)
                        {
                            node.Generate(eColorType.Black,live);
                        }
                    }
                    else
                    {
                        if (node.GetColorStatus() == eEntityStatus.Generating)
                        {
                            node.StopGenerate();
                        }
                    }
                    break;
                }
                case eColorType.Black:
                {
                    var Neighbors = GetNodeNeighbors(node);
                    var blackEntityCount = Neighbors.Count(node => node.GetColorType() == eColorType.Black);
                    if (blackEntityCount < 2 || blackEntityCount > 3)
                    {
                        willDieNodes.Add(node);
                    }
                    else if(node.GetColorStatus() == eEntityStatus.Dying)
                    {
                        node.StopDie();
                    }
                    break;
                }
            }
        }

        for (int i = 0; i < willDieNodes.Count; i++)
        {
            willDieNodes[i].StartDie();
        }
    }

    public List<BlockNode> GetNodeNeighbors(BlockNode node)
    {
        List<BlockNode> blockNodes = new List<BlockNode>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;
                var pos = node.pos + new Vector2Int(i, j);
                if (PosInRange(pos))
                {
                    blockNodes.Add(nodes[pos.x,pos.y]);
                }
            }
        }
        return blockNodes;
    }

    public bool PosInRange(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
    }
}
