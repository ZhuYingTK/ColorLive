using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public void Awake()
    {
        Instance = this;
        BlackCellData = AssetDatabase.LoadAssetAtPath<CellDataScriptableObject>("Assets/DataRes/BlackCell.asset");
    }

    public static CellDataScriptableObject BlackCellData { get; private set; }
}
