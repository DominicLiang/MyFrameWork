using QFramework;

public class CustomItemListRenameCommand : AbstractCommand
{
    private readonly int index;
    private readonly string newName;

    public CustomItemListRenameCommand(int index, string newName)
    {
        this.index = index;
        this.newName = newName;
    }

    protected override void OnExecute()
    {
        var customItemList = this.GetModel<IInventoryModel>().CustomItemLists;
        customItemList[index].listName = newName;
    }
}