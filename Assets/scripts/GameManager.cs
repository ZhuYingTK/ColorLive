using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int blackRes
    {
        set
        {
            _blackRes = value;
            blackResText.SetText(_blackRes.ToString());
        }
        get => _blackRes;
    }
    private int _blackRes = 1;
    public TMP_Text blackResText;

    public void Awake()
    {
        Instance = this;
        BlackTileData = AssetDatabase.LoadAssetAtPath<TileDataScriptableObject>("Assets/DataRes/BlackCell.asset");
    }

    public static TileDataScriptableObject BlackTileData { get; private set; }
}
