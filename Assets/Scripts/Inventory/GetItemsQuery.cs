using System.Collections.Generic;
using QFramework;

public class GetItemsQuery : AbstractQuery<List<Item>>
{
    protected override List<Item> OnDo()
    {
        return this.GetModel<IInventoryModel>().Items;
    }
}