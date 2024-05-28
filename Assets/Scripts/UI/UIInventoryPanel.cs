using System.Collections.Generic;
using QFramework;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class UIInventoryPanel : MonoBehaviour, IController
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

    public int slotCount = 72;

    private Transform slotRoot;
    private GameObject slotPrefab;
    private GameObject[] slots;
    private List<Item> items;

    private void Awake()
    {
        slotRoot = transform.Find("Panel/SlotRoot");

        var handle = Addressables.LoadAssetAsync<GameObject>("Slot");
        slotPrefab = handle.WaitForCompletion();

        slots = new GameObject[slotCount];
        items = this.SendQuery(new GetItemsQuery());
    }

    private void Start()
    {
        InitSlots();
        RefreshItem();
    }

    private void InitSlots()
    {
        for (int i = 0; i < slotCount; i++)
        {
            var slot = Instantiate(slotPrefab, slotRoot);
            slots[i] = slot;
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

    private void RefreshToolTip()
    {
        if (selectedItem != null)
        {
            // todo 打开并更新tooltip
        }
        else
        {
            // todo 关闭tooltip
        }
    }

    public IArchitecture GetArchitecture()
    {
        return App.Interface;
    }
}
