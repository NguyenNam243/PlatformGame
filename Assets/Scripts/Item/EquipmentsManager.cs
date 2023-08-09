using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static HeroData;

public class EquipmentsManager : MonoBehaviour
{
    public GameObject uiItemPrefab = null;
    public RectTransform itemsParent = null;
    public TMP_Text hpTxt = null;
    public TMP_Text atkTxt = null;
    public TMP_Text dfsTxt = null;
    public TMP_Text spdTxt = null;


    private List<ItemData> itemsOwned = null;
    private HeroData heroData = null;

    private List<ToggleElement> toggleElements = null;
    private List<UI_Item> itemsVisible = new List<UI_Item>();
    private List<UI_Equipment> uiEquipments = null;



    private void Awake()
    {
        itemsOwned = Resources.Load<ItemsStore>("Data/ItemsData/ItemsData").items;
        heroData = Resources.Load<HeroStore>("Data/HeroData/HeroData").heroData;

        LoadHeroAttributes();
        StartCoroutine(SpawnUIItem());

        toggleElements = GetComponentsInChildren<ToggleElement>().ToList();
        uiEquipments = GetComponentsInChildren<UI_Equipment>().ToList();

        foreach (var item in uiEquipments)
            item.OnUnEquip = OnUnEquipCostume;

        foreach (var element in toggleElements)
            element.OnSelected = OnTabSelected;

        toggleElements[0].IsOn = true;
    }

    private void LoadHeroAttributes()
    {
        float hp = heroData.HP;
        float atk = heroData.ATK;
        float dfs = heroData.DFS;
        float spd = heroData.SPD;

        if (heroData.costumes.Count > 0)
        {
            foreach (var costume in heroData.costumes)
            {
                hpTxt.text = (hp += costume.itemData.HP).ToString();
                atkTxt.text = (atk += costume.itemData.ATK).ToString();
                dfsTxt.text = (dfs += costume.itemData.DFS).ToString();
                spdTxt.text = (spd += costume.itemData.SPD).ToString();
            }
        }
        else
        {
            hpTxt.text = hp.ToString();
            atkTxt.text = atk.ToString();
            dfsTxt.text = dfs.ToString();
            spdTxt.text = spd.ToString();
        }
    }

    public void OnEquipCostume(ItemData itemData)
    {
        Costume costume = heroData.costumes.Find(c => c.type == itemData.type);

        if (costume != null)
        {
            costume.itemData = itemData;
        }
        else
        {
            costume = new Costume();
            costume.type = itemData.type;
            costume.itemData = itemData;
            heroData.costumes.Add(costume);
        }

        UI_Equipment targetEquipment = uiEquipments.Find(e => e.type == itemData.type);
        targetEquipment.Initialized(itemData);
        LoadHeroAttributes();
    }

    private void OnUnEquipCostume(ItemData itemData)
    {
        Costume costume = heroData.costumes.Find(c => c.type == itemData.type);
        if (costume != null)
        {
            heroData.costumes.Remove(costume);
            uiEquipments.Find(u => u.type == itemData.type).UnEquip();
            LoadHeroAttributes();
        }
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
