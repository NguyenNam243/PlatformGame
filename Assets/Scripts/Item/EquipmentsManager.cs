using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentsManager : MonoBehaviour
{
    public GameObject uiItemPrefab = null;
    public RectTransform itemsParent = null;

    private List<ItemData> itemsOwned = null;


    private void Awake()
    {
        itemsOwned = Resources.Load<ItemsStore>("Data/ItemsData/ItemsData").items;

        foreach (var item in itemsOwned)
        {
            GameObject itemPrefab = Instantiate(uiItemPrefab, itemsParent);
            UI_Item uiScript = itemPrefab.GetComponent<UI_Item>();
            uiScript.Initialized(item);
        }
    }
}
