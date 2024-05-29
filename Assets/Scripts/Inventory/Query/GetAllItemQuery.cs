using System.Collections.Generic;
using QFramework;

public class GetAllItemQuery : AbstractQuery<List<Item>>
{
    private readonly bool isBackpack;

    public GetAllItemQuery(bool isBackpack)
    {
        this.isBackpack = isBackpack;
    }

    protected override List<Item> OnDo()
    {
        var inventory = this.GetModel<IInventoryModel>();
        return isBackpack ? inventory.Backpack : inventory.Storage;
    }
}