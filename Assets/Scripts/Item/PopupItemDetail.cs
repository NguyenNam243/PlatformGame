using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupItemDetail : MonoBehaviour
{
    public EquipmentsManager equipmentsManager = null;
    public Button closeBtn = null;
    public Button equipBtn = null;
    public Image icon = null;
    public TMP_Text itemType = null;
    public TMP_Text itemRarity = null;
    public TMP_Text itemName = null;
    public TMP_Text HPText = null;
    public TMP_Text ATKText = null;
    public TMP_Text DFSText = null;
    public TMP_Text SPDText = null;


    private readonly string itemPath = "Sprites/Items/{0}/{1}";
    private UIAnimation _animation = null;

    private CanvasGroup canvasGr = null;

    private ItemData itemData = null;


    private void Awake()
    {
        _animation = GetComponent<UIAnimation>();
        canvasGr = GetComponent<CanvasGroup>();
        closeBtn.onClick.AddListener(HidePopup);
        equipBtn.onClick.AddListener(OnEquipClicked);
    }

    private void OnEquipClicked()
    {
        equipmentsManager.OnEquipCostume(itemData);
        HidePopup();
    }

    public void ShowItemDetail(ItemData itemData)
    {
        this.itemData = itemData;

        icon.sprite = Resources.Load<Sprite>(string.Format(itemPath, itemData.type.ToString(), itemData.itemName));
        itemType.text = itemData.type.ToString();
        itemRarity.text = itemData.rarity.ToString();
        itemName.text = itemData.itemName.ToString();

        HPText.text = itemData.HP.ToString();
        ATKText.text = itemData.ATK.ToString();
        DFSText.text = itemData.DFS.ToString();
        SPDText.text = itemData.SPD.ToString();

        canvasGr.Active();
        _animation.DoAnimation();
    }

    public void HidePopup()
    {
        canvasGr.DeActive();
    }
}


public static class GameUtilities
{
    public static void Active(this CanvasGroup cvGr)
    {
        cvGr.alpha = 1;
        cvGr.blocksRaycasts = cvGr.interactable = true;
    }

    public static void DeActive(this CanvasGroup cvGr)
    {
        cvGr.alpha = 0;
        cvGr.blocksRaycasts = cvGr.interactable = false;
    }
}
