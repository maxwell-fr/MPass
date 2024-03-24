using System.Collections;

namespace MPass;

public class Passphrase : IEnumerable<string>
{
    private readonly IList<ITokenSource> _tokenSequence;
    private readonly bool _shuffle;
    private readonly Random _rng;

    public Passphrase(IList<ITokenSource> tokenSequence, bool shuffle = false)
    {
        _tokenSequence = tokenSequence;
        _rng = new Random();
        _shuffle = shuffle;
    }

    public string GetPassphrase()
    {
        var seq = Enumerable.Range(0, _tokenSequence.Count);
        if (_shuffle)
        {
            seq = seq.OrderBy(r => _rng.Next());
        }

        return string.Concat(_tokenSequence.Zip(seq, (n, i) => (n, i))
                                            .OrderBy(t => t.Item2)
                                            .Select(t => t.Item1.GetToken())
                                            .ToList());
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
