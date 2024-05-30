using System.Collections.Generic;
using System.Linq;
using QFramework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIStoragePanel : MonoBehaviour, IController
{
    private Item selectedItem;
    public Item SelectedItem
    {
        get => selectedItem;
        set
        {
            if (selectedItem == value) return;
            selectedItem = value;
            RefreshToolTip();
        }
    }

    private GameObject nextSelect;
    public GameObject NextSelect
    {
        get => nextSelect;
        set
        {
            if (nextSelect == value) return;
            nextSelect = value;
            otherStoragePanel.SetReallyToSelect(value);
        }
    }

    public int slotCount;
    public bool isBackpack;
    [HideInInspector]
    public UIStoragePanel otherStoragePanel;
    [HideInInspector]
    public GameObject reallyToSelect;

    private Transform slotRoot;
    private Image background;
    private GameObject slotPrefab;
    private GameObject[] slots;
    private List<Item> items;

    private void Awake()
    {
        slotRoot = transform.Find("Panel/SlotRoot");
        background = transform.Find("Panel/Background").GetComponent<Image>();

        var handle = Addressables.LoadAssetAsync<GameObject>("Slot");
        slotPrefab = handle.WaitForCompletion();

        slots = new GameObject[slotCount];
        items = this.SendQuery(new GetAllItemQuery(isBackpack));
    }

    private void Start()
    {
        InitSlots();
        RefreshItem();

        if (isBackpack)
        {
            SetSelectedGameObject(slots[0]);
            Selected(true);
            otherStoragePanel.Selected(false);
        }

        this.RegisterEvent<ItemChangeEvent>((e) =>
        {
            if (e.isBackpack != isBackpack) return;
            RefreshItem();
        }).UnRegisterWhenGameObjectDestroyed(this);
    }

    private void InitSlots()
    {
        for (int i = 0; i < slotCount; i++)
        {
            var slot = Instantiate(slotPrefab, slotRoot);
            slots[i] = slot;
        }

        if (isBackpack)
        {
            SetNavigation(4, 15);
        }
        else
        {
            SetNavigation(7, 64);
        }
    }

    private void SetNavigation(int right, int up)
    {
        var leftIndex = 0;
        var rightIndex = right;
        for (int i = 0; i < slots.Length; i++)
        {
            var button = slots[i].GetComponent<Button>();

            var nav = new Navigation { mode = Navigation.Mode.Explicit };

            if (i + 1 < slots.Length) nav.selectOnRight = slots[i + 1].GetComponent<Button>();
            if (i - 1 >= 0) nav.selectOnLeft = slots[i - 1].GetComponent<Button>();
            if (i - (right + 1) >= 0) nav.selectOnUp = slots[i - (right + 1)].GetComponent<Button>();
            if (i + right + 1 < slots.Length) nav.selectOnDown = slots[i + right + 1].GetComponent<Button>();

            if (i == rightIndex)
            {
                rightIndex += right + 1;
                nav.selectOnRight = slots[i - right].GetComponent<Button>();
            }
            if (i == leftIndex && i + right < slots.Length)
            {
                leftIndex += right + 1;
                nav.selectOnLeft = slots[i + right].GetComponent<Button>();
            }
            if (i <= right) nav.selectOnUp = slots[i + up].GetComponent<Button>();
            if (i >= up) nav.selectOnDown = slots[i - up].GetComponent<Button>();

            button.navigation = nav;
        }
    }

    private void RefreshToolTip()
    {
        if (selectedItem != null && selectedItem.count > 0)
        {
            UIManager.Instance.OpenPanel(PanelID.TooltipPanel, out GameObject panel);
            panel.GetComponent<UITooltipPanel>().Refresh(selectedItem);
        }
        else
        {
            UIManager.Instance.ClosePanel(PanelID.TooltipPanel);
        }
    }

    public void RefreshItem()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < items.Count)
            {
                slots[i].GetComponent<UISlot>().Refresh(items[i]);
            }
            else
            {
                slots[i].GetComponent<UISlot>().Refresh(null);
            }
        }
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

    public void SetReallyToSelect(GameObject next)
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
        reallyToSelect = select;
    }

    public void SetSelectedGameObject(GameObject next)
    {
        if (next == null) next = slots[0];
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(next);
    }

    public IArchitecture GetArchitecture()
    {
        return App.Interface;
    }
}
