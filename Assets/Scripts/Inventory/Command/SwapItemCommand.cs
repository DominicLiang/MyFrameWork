using QFramework;

public class SwapItemCommand : AbstractCommand
{
    private readonly int itemOneIndex;
    private readonly int itemTwoIndex;
    private readonly bool isBackpack;

    public SwapItemCommand(int itemOneIndex, int itemTwoIndex, bool isBackpack)
    {
        this.itemOneIndex = itemOneIndex;
        this.itemTwoIndex = itemTwoIndex;
        this.isBackpack = isBackpack;
    }

    protected override void OnExecute()
    {
        var model = this.GetModel<IInventoryModel>();
        var items = isBackpack ? model.Backpack : model.Storage;

        if (itemOneIndex == itemTwoIndex || itemOneIndex == -1 || itemTwoIndex == -1) return;
        (items[itemTwoIndex], items[itemOneIndex]) = (items[itemOneIndex], items[itemTwoIndex]);
        this.SendEvent(new ItemChangeEvent(null, isBackpack));
    }
}