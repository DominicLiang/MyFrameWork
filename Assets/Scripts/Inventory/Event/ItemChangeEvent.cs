public struct ItemChangeEvent
{
    public readonly Item item;
    public bool isBackpack;

    public ItemChangeEvent(Item item, bool isBackpack) : this()
    {
        this.item = item;
        this.isBackpack = isBackpack;
    }
}