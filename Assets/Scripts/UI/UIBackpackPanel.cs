using System;
using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBackpackPanel : UIStorageBasePanel, IController
{
    private const string SelectItemOne = "SelectItemOne";
    private const string SelectItemTwo = "SelectItemTwo";
    private string currentState = "SelectItemOne";
    private int indexOne = -1;
    private UISlot hightLightSlot;

    protected override void Start()
    {
        base.Start();

        SetSelectedGameObject(slots[0]);

        SetSlotEvent();

        SetupInput();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SetSelectedGameObject(slots[0]);
        if (slots[0] != null) slots[0].GetComponent<UISlot>().OnSelect(null);
    }

    private void SetupInput()
    {
        west.performed += (e) => OpenSubMenu();
        north.performed += (e) => this.SendCommand(new SortItemCommand(true));
    }

    private void OpenSubMenu()
    {
        if (SelectedItem.count <= 0) return;
        UIManager.Instance.OpenPanel(PanelID.BackpackSubMenuPanel, out var panel);

        var panelComponent = panel.GetComponent<UIBackpackSubMenuPanel>();
        panelComponent.Setup(SelectedItem, NextSelect);

        var targetRectTransform = NextSelect.GetComponentInParent<RectTransform>();
        var baseRectTransform = transform.Find("Panel").GetComponent<RectTransform>();
        var uiRectTransform = panel.transform.Find("Panel").GetComponent<RectTransform>();
        var target = targetRectTransform.anchoredPosition;
        target += baseRectTransform.anchoredPosition;
        target += new Vector2(-50, 140);
        uiRectTransform.anchoredPosition = target;
    }

    private void SetSlotEvent()
    {
        foreach (var slot in slots)
        {
            var uiSlot = slot.GetComponent<UISlot>();
            uiSlot.OnSubmitEvent += OnSubmitEvent;
            uiSlot.OnDestroyEvent += (e) => e.OnSubmitEvent -= OnSubmitEvent;
        }

        void OnSubmitEvent(Item item)
        {
            switch (currentState)
            {
                case SelectItemOne:
                    var go1 = EventSystem.current.currentSelectedGameObject;
                    hightLightSlot = go1.GetComponent<UISlot>();
                    hightLightSlot.SetSwapHighLight(true);
                    indexOne = Array.IndexOf(slots, go1);
                    SelectedItem = null;
                    currentState = SelectItemTwo;
                    break;
                case SelectItemTwo:
                    var go2 = EventSystem.current.currentSelectedGameObject;
                    hightLightSlot.SetSwapHighLight(false);
                    var indexTwo = Array.IndexOf(slots, go2);
                    this.SendCommand(new SwapItemCommand(indexOne, indexTwo, true));
                    indexOne = -1;
                    hightLightSlot = null;
                    SelectedItem = slots[indexTwo].GetComponent<UISlot>().item;
                    currentState = SelectItemOne;
                    break;
                default:
                    break;
            }
        }
    }
}