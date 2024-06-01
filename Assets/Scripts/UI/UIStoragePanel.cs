using System.Linq;
using QFramework;
using UnityEngine;

public class UIStoragePanel : UIStorageBasePanel, IController
{
    private UIStoragePanel otherStoragePanel;
    public UIStoragePanel OtherStoragePanel
    {
        get
        {
            if (otherStoragePanel != null) return otherStoragePanel;
            var id = isBackpack ? "Storage" : "Backpack";
            var other = transform.parent.Find(id).GetComponent<UIStoragePanel>();
            otherStoragePanel = other;
            return otherStoragePanel;
        }
    }

    [HideInInspector]
    public GameObject readyToSelect;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (isBackpack)
        {
            if (slots[0] != null) slots[0].GetComponent<UISlot>().OnSelect(null);
            SetSelectedGameObject(slots[0]);
            Selected(true);
        }
    }

    protected override void Start()
    {
        base.Start();

        if (isBackpack)
        {
            OtherStoragePanel.Selected(false);
            SetSelectedGameObject(slots[0]);
            Selected(true);
        }

        SetSlotEvent();

        NextSelectEvent += () => OtherStoragePanel.SetReadyToSelect(NextSelect);

        leftShoulder.performed += (e) =>
        {
            if (isBackpack) return;
            OtherStoragePanel.SetSelectedGameObject(OtherStoragePanel.readyToSelect);
            OtherStoragePanel.Selected(true);
            Selected(false);
        };

        rightShoulder.performed += (e) =>
        {
            if (!isBackpack) return;
            OtherStoragePanel.SetSelectedGameObject(OtherStoragePanel.readyToSelect);
            OtherStoragePanel.Selected(true);
            Selected(false);
        };

        north.performed += (e) =>
        {
            if (!isBackpack) return;

            this.SendCommand(new SortItemCommand(background.color == Color.white));
        };
    }

    public void Selected(bool isSelected)
    {
        background.color = isSelected ? Color.white : Color.gray;
        foreach (var slot in slots)
        {
            if (slot == null) continue;
            slot.GetComponent<UISlot>().Selected(isSelected);
        }
    }

    public void SetReadyToSelect(GameObject next)
    {
        Selected(false);
        var select = slots.FirstOrDefault(x =>
        {
            var xComponent = x.GetComponent<UISlot>();
            var nextComponent = next.GetComponent<UISlot>();
            if (xComponent.item.itemData == null || nextComponent.item.itemData == null) return false;
            return xComponent.item.itemData.id == nextComponent.item.itemData.id;
        });
        if (select == null) return;
        select.GetComponent<UISlot>().Selected(true);
        readyToSelect = select;
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
            if (item.itemData != null)
            {
                this.SendCommand(new StoreItemCommand(item.itemData.id, item.count, isBackpack));
            }
            else
            {
                OtherStoragePanel.SetSelectedGameObject(OtherStoragePanel.readyToSelect);
                OtherStoragePanel.Selected(true);
                Selected(false);
            }
        }
    }
}

