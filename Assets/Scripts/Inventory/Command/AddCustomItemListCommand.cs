using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class AddCustomItemListCommand : AbstractCommand
{
    private readonly int index;
    private readonly List<Item> items;

    public AddCustomItemListCommand(int index, List<Item> items)
    {
        this.index = index;
        this.items = items;
    }

    protected override void OnExecute()
    {
        var customItemList = this.GetModel<IInventoryModel>().CustomItemLists;
        customItemList[index] = new CustomItemList()
        {
            listName = $"个人组合{index}",
            customItems = items
        };
    }
}