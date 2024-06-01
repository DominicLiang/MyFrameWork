using QFramework;

public class RemoveCustomItemListCommand : AbstractCommand
{
    private readonly int index;

    public RemoveCustomItemListCommand(int index)
    {
        this.index = index;
    }

    protected override void OnExecute()
    {
        var customItemList = this.GetModel<IInventoryModel>().CustomItemLists;
        customItemList[index] = new CustomItemList();
    }
}