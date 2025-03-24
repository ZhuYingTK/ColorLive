using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceItemUI : MonoBehaviour
{
    public List<ItemCard> cards = new List<ItemCard>();

    public void Start()
    {
        foreach (ItemCard card in cards)
        {
            card.button.onClick.AddListener(()=>OnClickCard(card));
        }
    }

    public void OnClickCard(ItemCard card)
    {
        
    }
}
