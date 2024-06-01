using QFramework;

public class StoreItemCommand : AbstractCommand
{
    private readonly int id;
    private readonly int num;
    private readonly bool isStore;

    public StoreItemCommand(int id, int num, bool isStore)
    {
        this.id = id;
        this.num = num;
        this.isStore = isStore;
    }

    protected override void OnExecute()
    {
        var returnNum = this.SendCommand(new AddItemCommand(id, num, !isStore, true));
        var subNum = num - returnNum;
        this.SendCommand(new SubItemCommand(id, subNum, isStore, true));
    }
}