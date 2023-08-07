using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentsManager : MonoBehaviour
{
    public GameObject uiItemPrefab = null;
    public RectTransform itemsParent = null;

    private List<ItemData> itemsOwned = null;

    private List<ToggleElement> toggleElements = null;
    private List<UI_Item> itemsVisible = new List<UI_Item>();



    private void Awake()
    {
        itemsOwned = Resources.Load<ItemsStore>("Data/ItemsData/ItemsData").items;

        StartCoroutine(SpawnUIItem());

        toggleElements = GetComponentsInChildren<ToggleElement>().ToList();

        foreach (var element in toggleElements)
            element.OnSelected = OnTabSelected;

        toggleElements[0].IsOn = true;
    }

    private IEnumerator SpawnUIItem()
    {
        foreach (var item in itemsOwned)
        {
            GameObject itemPrefab = Instantiate(uiItemPrefab, itemsParent);
            UI_Item uiScript = itemPrefab.GetComponent<UI_Item>();
            uiScript.Initialized(item);
            itemsVisible.Add(uiScript);
            yield return new WaitForSeconds(0.02f);
        }
    }

    private void OnTabSelected(ItemType type)
    {
        foreach (var item in itemsVisible)
        {
            item.gameObject.SetActive(type == ItemType.All || item.ItemData.type == type);
        }
    }
}

public enum ItemType
{
    All,
    Weapon,
    Armor,
    Boots,
    ring
}
