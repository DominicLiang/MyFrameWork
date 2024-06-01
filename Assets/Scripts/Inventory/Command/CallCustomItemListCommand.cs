using QFramework;

public class CallCustomItemListCommand : AbstractCommand
{
    private readonly int index;

    public CallCustomItemListCommand(int index)
    {
        this.index = index;
    }

    protected override void OnExecute()
    {
        var inventoryModel = this.GetModel<IInventoryModel>();
        var customItemList = inventoryModel.CustomItemLists;
        var items = customItemList[index].customItems;

        foreach (var item in items)
        {
            var itemFromBackpack = this.SendQuery(new GetItemByIdQuery(item.itemData.id, true));
            var needSub = item.count - (itemFromBackpack == null ? 0 : itemFromBackpack.count);
            if (needSub <= 0) continue;
            var subRemain = this.SendCommand(new SubItemCommand(item.itemData.id, needSub, false, false));
            var realSub = needSub - subRemain;
            if (realSub <= 0) continue;
            this.SendCommand(new AddItemCommand(item.itemData.id, realSub, true, false));
        }

        this.SendEvent(new ItemChangeEvent(null, true));
        this.SendEvent(new ItemChangeEvent(null, false));
    }
}