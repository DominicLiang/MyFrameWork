using System.Collections.Generic;
using QFramework;

public interface IInventoryModel : IModel
{
    List<Item> Items { get; }
}