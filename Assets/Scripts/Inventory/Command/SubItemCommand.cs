using QFramework;

public class SubItemCommand : AbstractCommand<int>
{
    private readonly int id;
    private readonly int num;
    private readonly bool isBackpack;

    public SubItemCommand(int id, int num, bool isBackpack)
    {
        this.id = id;
        this.num = num;
        this.isBackpack = isBackpack;
    }

    protected override int OnExecute()
    {
        var inventory = this.GetModel<IInventoryModel>();
        var items = isBackpack ? inventory.Backpack : inventory.Storage;

        var itemToRemove = items.Find(x => x.itemData != null && x.itemData.id == id);

        if (itemToRemove == null) return num;

        if (num >= itemToRemove.count)
        {
            var returnItem = num - itemToRemove.count;
            itemToRemove.count = 0;
            itemToRemove.itemData = null;

            this.SendEvent(new ItemChangeEvent(itemToRemove, isBackpack));

            return returnItem;
        }
        else
        {
            itemToRemove.count -= num;

            this.SendEvent(new ItemChangeEvent(itemToRemove, isBackpack));

            return 0;
        }
    }
}