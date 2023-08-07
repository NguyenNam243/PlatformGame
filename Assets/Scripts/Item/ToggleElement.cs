using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleElement : MonoBehaviour
{
    public ItemType type;
    public CanvasGroup iconNormal = null;
    public CanvasGroup iconFocus = null;
    public CanvasGroup tabFocus = null;


    private Toggle toggle = null;

    public bool IsOn 
    {
        get => toggle.isOn;
        set => toggle.isOn = value;
    }

    public Action<ItemType> OnSelected = null;

    public bool Selected { get; private set; }


    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnToggleChanged(bool isOn)
    {
        if (type != ItemType.All)
        {
            iconNormal.alpha = !isOn ? 1 : 0;
            iconFocus.alpha = isOn ? 1 : 0;
        }
        else
        {
            GetComponentInChildren<TMP_Text>().color = isOn ? new Color32(246, 225, 156, 255) : new Color(1, 1, 1, 1);
        }
       
        tabFocus.alpha = isOn ? 1 : 0; ;
        Selected = isOn;

        if (isOn)
            OnSelected?.Invoke(type);
    }


}
