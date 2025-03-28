using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ItemCard : MonoBehaviour
{
    public Image image;
    public Button button;
    public Transform costContainer;
    public GameObject costCardPrefab;
    public List<ResCostScritableObject> costSO = new List<ResCostScritableObject>();
    private List<CostCard> costCards = new List<CostCard>();
    private PlaceableItem _placeable;

    public void Init()
    {
        _placeable = new CellPlaceableItem(costSO);
        for (int i = 0; i < costSO.Count; i++)
        {
            var costCard = Instantiate(costCardPrefab, costContainer).GetComponent<CostCard>();
            costCard.Init(costSO[i]);
            costCards.Add(costCard);
        }
    }

    public void UpdateDataAndUI()
    {
        _placeable.Update();
        for (int i = 0; i < _placeable.Cost.Count; i++)
        {
            costCards[i].SetCountText(_placeable.Cost[i].Count);
        }
    }
}
