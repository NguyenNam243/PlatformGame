using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    [Header("Popup Item Information")]
    public PopupItemDetail popupItemDetail = null;



    public static PopupManager Instance = null;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(Instance);
    }


    public void ShowPopupItemDetail(ItemData data)
    {
        popupItemDetail.ShowItemDetail(data);
    }

    public void HidePopupItemDetail()
    {
        popupItemDetail.HidePopup();
    }
}
