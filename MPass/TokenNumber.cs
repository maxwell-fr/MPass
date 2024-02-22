using System.Collections;
using System.Text;

namespace MPass;

public class TokenNumber : ITokenSource
{
    private readonly int _digits;

    public Random Random { private get; set; } = new Random();

    public TokenNumber(int digits)
    {
        _digits = digits;
    }

    public string GetToken()
    {
        var sb = new StringBuilder();

        for (var i = 0; i < _digits; ++i)
        {
            sb.Append(Random.Next(10).ToString());
        }

        return sb.ToString();
    }

    public IEnumerator<string> GetEnumerator()
    {
        while (true)
        {
            yield return GetToken();
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
