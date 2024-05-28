using QFramework;

public class App : Architecture<App>
{
    protected override void Init()
    {
        RegisterModel<IInventoryModel>(new InventoryModel());
    }
}

