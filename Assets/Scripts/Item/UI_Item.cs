using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Item : MonoBehaviour
{
    public Image border = null;
    public Image bgImage = null;
    public Image itemicon = null;
    public Color[] backgroundColor;

    private readonly string uiItemAssetsPath = "Sprites/{0}/{1}";


    public void Initialized(ItemData itemData)
    {
        border.sprite = Resources.Load<Sprite>(string.Format(uiItemAssetsPath, "Border", itemData.rarity.ToString()));
        bgImage.color = GetColorByRarity(itemData.rarity);
        itemicon.sprite = Resources.Load<Sprite>(string.Format(uiItemAssetsPath, "Items", itemData.itemName));
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
}
