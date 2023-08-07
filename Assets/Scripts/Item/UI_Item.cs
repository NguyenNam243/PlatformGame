using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Item : MonoBehaviour, IPointerDownHandler
{
    public Image border = null;
    public Image bgImage = null;
    public Image itemicon = null;
    public Color[] backgroundColor;

    private readonly string borderPath = "Sprites/Border/{0}";
    private readonly string itemPath = "Sprites/Items/{0}/{1}";

    public ItemData ItemData { get; private set; }


    public bool IsHold { get; private set; }


    public void Initialized(ItemData itemData)
    {
        this.ItemData = itemData;
        border.sprite = Resources.Load<Sprite>(string.Format(borderPath, itemData.rarity.ToString()));
        bgImage.color = GetColorByRarity(itemData.rarity);
        itemicon.sprite = Resources.Load<Sprite>(string.Format(itemPath, itemData.type.ToString(), itemData.itemName));

    }

    private Color GetColorByRarity(Rarity rarity)
    {
        switch(rarity)
        {
            case Rarity.Common:
                return backgroundColor[0];
            case Rarity.Rare:
                return backgroundColor[1];
            case Rarity.Epic:
                return backgroundColor[2];
            case Rarity.Legendary:
                return backgroundColor[3];
        }
        return backgroundColor[0];
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsHold = true;
        PopupManager.Instance.ShowPopupItemDetail(ItemData);
    }
}
