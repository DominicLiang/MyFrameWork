using System.Collections.Generic;
using QFramework;

public interface IInventoryModel : IModel
{
    Dictionary<int, ItemData> ItemDatabase { get; }
    List<Item> Backpack { get; }
    List<Item> Storage { get; }
}