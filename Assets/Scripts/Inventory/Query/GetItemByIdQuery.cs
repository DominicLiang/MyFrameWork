using QFramework;

public class GetItemByIdQuery : AbstractQuery<Item>
{
    private readonly int id;
    private readonly bool isBackpack;

    public GetItemByIdQuery(int id, bool isBackpack)
    {
        this.id = id;
        this.isBackpack = isBackpack;
    }

    protected override Item OnDo()
    {
        var inventory = this.GetModel<IInventoryModel>();
        var items = isBackpack ? inventory.Backpack : inventory.Storage;
        return items.Find(x => x.itemData != null && x.itemData.id == id);
    }
}