using System.Collections.Generic;
using QFramework;
using UnityEngine.AddressableAssets;

public class InventoryModel : AbstractModel, IInventoryModel
{
    public Dictionary<int, ItemData> ItemDatabase { get; private set; }
    public List<Item> Backpack { get; private set; }
    public List<Item> Storage { get; private set; }

    protected override void OnInit()
    {
        ItemDatabase = new Dictionary<int, ItemData>();
        var handle = Addressables.LoadAssetAsync<ItemDatabase>("ItemDatabase");
        var itemData = handle.WaitForCompletion().items;
        foreach (var item in itemData)
        {
            ItemDatabase.Add(item.id, item);
        }

        // todo FakeData must delete
        Backpack = new List<Item>();
        for (int i = 0; i < 20; i++)
        {
            switch (i)
            {
                case 0:
                    Backpack.Add(new Item()
                    {
                        itemData = ItemDatabase[2],
                        count = 1,
                        level = 1
                    });
                    break;
                case 1:
                    Backpack.Add(new Item()
                    {
                        itemData = ItemDatabase[8],
                        count = 12,
                        level = 1
                    });
                    break;
                case 2:
                    Backpack.Add(new Item()
                    {
                        itemData = ItemDatabase[3],
                        count = 1,
                        level = 12
                    });
                    break;
                case 3:
                    Backpack.Add(new Item()
                    {
                        itemData = ItemDatabase[10],
                        count = 30,
                        level = 1
                    });
                    break;
                case 4:
                    Backpack.Add(new Item()
                    {
                        itemData = ItemDatabase[4],
                        count = 1,
                        level = 3
                    });
                    break;
                default:
                    Backpack.Add(new Item());
                    break;
            }
        }

        Storage = new List<Item>();
        for (int i = 0; i < 72; i++)
        {
            switch (i)
            {
                case 0:
                    Storage.Add(new Item()
                    {
                        itemData = ItemDatabase[8],
                        count = 20,
                        level = 1
                    });
                    break;
                case 1:
                    Storage.Add(new Item()
                    {
                        itemData = ItemDatabase[6],
                        count = 67,
                        level = 1
                    });
                    break;
                case 2:
                    Storage.Add(new Item()
                    {
                        itemData = ItemDatabase[4],
                        count = 1,
                        level = 12
                    });
                    break;
                default:
                    Storage.Add(new Item());
                    break;
            }
        }
        // todo FakeData must delete
    }
}