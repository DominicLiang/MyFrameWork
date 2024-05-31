using System.Collections.Generic;
using QFramework;

public class SortItemCommand : AbstractCommand
{
    private readonly bool isBackpack;

    public SortItemCommand(bool isBackpack)
    {
        this.isBackpack = isBackpack;
    }

    protected override void OnExecute()
    {
        var model = this.GetModel<IInventoryModel>();
        var items = isBackpack ? model.Backpack : model.Storage;

        items.Sort(new ItemComparer());

        this.SendEvent(new ItemChangeEvent(null, isBackpack));
    }

    class ItemComparer : IComparer<Item>
    {
        public int Compare(Item x, Item y)
        {
            if (x.count <= 0 && y.count <= 0) return 0;
            if (x.count <= 0) return 1;
            if (y.count <= 0) return -1;
            var typeCompare = x.itemData.itemType.CompareTo(y.itemData.itemType);
            if (typeCompare != 0) return typeCompare;
            var rareCompare = x.itemData.rare.CompareTo(y.itemData.rare);
            if (rareCompare != 0) return rareCompare * -1;
            var levelCompare = x.level.CompareTo(y.level);
            if (levelCompare != 0) return levelCompare * -1;
            var idCompare = x.itemData.id.CompareTo(y.itemData.id);
            return idCompare;
        }
    }
}