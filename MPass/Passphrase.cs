namespace MPass;

public class Passphrase
{
    private readonly string _passphrase;
    public override string ToString()
    {
        return _passphrase;
    }

    public Passphrase(IList<ITokenSource> tokenSequence)
    {
        _passphrase = string.Concat(tokenSequence.Select(t => t.GetToken()).ToList());
    }
}
