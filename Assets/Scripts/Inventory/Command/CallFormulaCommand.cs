using QFramework;

public class CallFormulaCommand : AbstractCommand
{
    private readonly int formulaId;
    private readonly int number;

    public CallFormulaCommand(int formulaId, int number)
    {
        this.formulaId = formulaId;
        this.number = number;
    }

    protected override void OnExecute()
    {
        throw new System.NotImplementedException();
    }
}