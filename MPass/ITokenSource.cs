namespace MPass;

public interface ITokenSource: IEnumerable<string>
{
    public string GetToken();
}
