using System.Collections;

namespace MPass;

/// <summary>
/// A token source that returns only a single space character.
/// </summary>
public class TokenSpace : ITokenSource
{
    public string GetToken()
    {
        return " ";
    }

    public IEnumerator<string> GetEnumerator()
    {
        while(true)
        {
            yield return GetToken();
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
