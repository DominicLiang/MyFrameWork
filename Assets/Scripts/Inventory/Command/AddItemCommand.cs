using QFramework;

public class AddItemCommand : AbstractCommand<int>
{
    private readonly int id;
    private readonly int num;
    private readonly bool isBackpack;
    private readonly bool needSendEvent;

    public AddItemCommand(int id, int num, bool isBackpack, bool needSendEvent)
    {
        this.id = id;
        this.num = num;
        this.isBackpack = isBackpack;
        this.needSendEvent = needSendEvent;
    }

    protected override int OnExecute()
    {
        var inventory = this.GetModel<IInventoryModel>();
        var itemDatabase = inventory.ItemDatabase;
        var items = isBackpack ? inventory.Backpack : inventory.Storage;

        var itemToAdd = items.Find(x => x.itemData != null && x.itemData.id == id);

        if (itemToAdd == null)
        {
            itemToAdd = items.Find(x => x.count <= 0);
            if (itemToAdd == null) return num;
            itemToAdd.itemData = itemDatabase.ContainsKey(id) ? itemDatabase[id] : null;
        }

        if (itemToAdd.itemData == null) return num;

        if (isBackpack)
        {
            if (itemToAdd.count + num <= itemToAdd.itemData.maxCarry)
            {
                itemToAdd.count += num;

                if (needSendEvent) this.SendEvent(new ItemChangeEvent(itemToAdd, isBackpack));

                return 0;
            }
            else
            {
                var returnItem = itemToAdd.count + num - itemToAdd.itemData.maxCarry;
                itemToAdd.count = itemToAdd.itemData.maxCarry;

                if (needSendEvent) this.SendEvent(new ItemChangeEvent(itemToAdd, isBackpack));

                return returnItem;
            }
        }
        else
        {
            itemToAdd.count += num;

            if (needSendEvent) this.SendEvent(new ItemChangeEvent(itemToAdd, isBackpack));

            return 0;
        }
    }
}