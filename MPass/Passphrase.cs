using System.Collections;

namespace MPass;

public class Passphrase : IEnumerable<string>
{
    private IList<ITokenSource> _tokenSequence;

    public Passphrase(IList<ITokenSource> tokenSequence)
    {
        _tokenSequence = tokenSequence;
    }

    public string GetPassphrase()
    {
        return string.Concat(_tokenSequence.Select(t => t.GetToken()).ToList());
    }

    public IEnumerator<string> GetEnumerator()
    {
        while (true)
        {
            yield return GetPassphrase();
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
