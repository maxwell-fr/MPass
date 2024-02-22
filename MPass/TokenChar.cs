using System.Collections;
using System.Text;

namespace MPass;

public class TokenChar : ITokenSource
{
    public const string UpperAsciiLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public const string LowerAsciiLetters = "abcdefghijklmnopqrstuvwxyz";
    public const string Numerals = "0123456789";
    public const string EzAsciiSymbols = "!@#$%^&*-=+";
    public const string AsciiSymbols = "!@#$%^&*()-_=+,./<>?;':\"[]\\{}|`~";

    public Random Random { private get; set; } = new Random();
    private readonly string _charset;


    /// <summary>
    /// Provide a token generator with the default charset.
    ///
    /// The default charset is upper and lowercase ASCII, numerals, and easy ASCII symbols.
    /// </summary>
    public TokenChar() : this(new[] { UpperAsciiLetters, LowerAsciiLetters, Numerals, EzAsciiSymbols })
    {
    }

    /// <summary>
    /// Provide a token generator with a custom charset.
    /// </summary>
    /// <param name="charset">An array of strings. The list of characters comprises the characters available
    /// for generation. Duplicates are allowed; de-duplication is not performed.</param>
    /// <exception cref="ArgumentException">Thrown if there are zero characters in the set.</exception>
    public TokenChar(string[] charset)
    {
        _charset = string.Concat(charset);
        if (_charset.Length < 1)
        {
            _charset = UpperAsciiLetters;
            throw new ArgumentException("No characters in charset.");
        }
    }

    public string GetToken()
    {
        var idx = Random.Next(_charset.Length);
        return _charset[idx].ToString();
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
