using System.Collections.Generic;
using QFramework;
using UnityEngine.AddressableAssets;

public class InventoryModel : AbstractModel, IInventoryModel
{
    public List<Item> Items { get; private set; }
    private Dictionary<int, ItemData> itemDatabase;

    protected override void OnInit()
    {
        Items = new List<Item>();
        itemDatabase = new Dictionary<int, ItemData>();
        var handle = Addressables.LoadAssetAsync<ItemDatabase>("ItemDatabase");
        var itemData = handle.WaitForCompletion().items;
        foreach (var item in itemData)
        {
            itemDatabase.Add(item.id, item);
        }

        // todo FakeData must delete
        Items.Add(new Item()
        {
            itemData = itemDatabase[1],
            count = 1,
            level = 1,
            isNew = false,
        });
        Items.Add(new Item()
        {
            itemData = itemDatabase[5],
            count = 7,
            level = 1,
            isNew = false,
        });
        Items.Add(new Item()
        {
            itemData = itemDatabase[3],
            count = 4,
            level = 1,
            isNew = false,
        });
        Items.Add(new Item()
        {
            itemData = itemDatabase[4],
            count = 2,
            level = 1,
            isNew = false,
        });
        Items.Add(new Item()
        {
            itemData = itemDatabase[2],
            count = 3,
            level = 1,
            isNew = false,
        });
        // todo FakeData must delete
    }
}