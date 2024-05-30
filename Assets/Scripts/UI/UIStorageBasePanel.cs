using System.Collections.Generic;
using QFramework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIStorageBasePanel : MonoBehaviour, IController
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

    public int rows;
    public int columns;
    public bool isBackpack;

    protected Transform slotRoot;
    protected Image background;
    protected GameObject slotPrefab;
    protected GameObject[] slots;
    protected List<Item> items;

    protected virtual void Awake()
    {
        slotRoot = transform.Find("Panel/SlotRoot");
        background = transform.Find("Panel/Background").GetComponent<Image>();

        var handle = Addressables.LoadAssetAsync<GameObject>("Slot");
        slotPrefab = handle.WaitForCompletion();

        slots = new GameObject[rows * columns];
        items = this.SendQuery(new GetAllItemQuery(isBackpack));
    }

    protected virtual void Start()
    {
        InitSlots();
        RefreshItem();

        this.RegisterEvent<ItemChangeEvent>((e) =>
        {
            if (e.isBackpack != isBackpack) return;
            RefreshItem();
        }).UnRegisterWhenGameObjectDestroyed(this);
    }

    protected void InitSlots()
    {
        for (int i = 0; i < rows * columns; i++)
        {
            var slot = Instantiate(slotPrefab, slotRoot);
            slots[i] = slot;
        }

        SetNavigation(columns - 1, rows * columns - columns);
    }

    protected void SetNavigation(int right, int up)
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

    protected void RefreshToolTip()
    {
        if (SelectedItem != null && SelectedItem.count > 0)
        {
            UIManager.Instance.OpenPanel(PanelID.TooltipPanel, out var panel);
            panel.GetComponent<UITooltipPanel>().Refresh(SelectedItem);
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

    public void SetSelectedGameObject(GameObject ready)
    {
        if (ready == null) ready = slots[0];
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(ready);
    }

    public IArchitecture GetArchitecture()
    {
        return App.Interface;
    }
}
