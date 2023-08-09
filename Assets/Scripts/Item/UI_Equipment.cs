using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_Equipment : MonoBehaviour
{
    public ItemType type;
    public Image icon = null;

    private readonly string itemPath = "Sprites/Items/{0}/{1}";

    private Sprite defaulIcon = null;
    private Button unEquipBtn = null;
    private ItemData itemData = null;

    public Action<ItemData> OnUnEquip = null;


    private void Awake()
    {
        if (icon != null)
            defaulIcon = icon.sprite;

        unEquipBtn = GetComponent<Button>();
        unEquipBtn.onClick.AddListener(UnEquip);
    }

    public void Initialized(ItemData itemData)
    {
        this.itemData = itemData;
        icon.sprite = Resources.Load<Sprite>(string.Format(itemPath, itemData.type.ToString(), itemData.itemName));
    }

    public void UnEquip()
    {
        icon.sprite = defaulIcon;
        OnUnEquip?.Invoke(itemData);
    }
}
