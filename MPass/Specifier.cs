using System.Text;

namespace MPass;

public class Specifier
{
    private TokenWord _words;
    private TokenChar _symbols;

    public Specifier(TokenWord words, TokenChar symbols)
    {
        _words = words;
        _symbols = symbols;
    }

    /// <summary>
    /// Parse a passphrase specification string.
    ///
    /// Specifier string syntax is simple.
    /// One or more of the following keys:
    ///     w - lowercase word (examnple)
    ///     u - uppercase word (EXAMPLE)
    ///     i - initial caps word (Example)
    ///     r - random cap word (exAmple)
    ///     x - random single alphanumeric character
    ///     z - random single alphanumeric or symbol character (combines x and $)
    ///     0 - digit, 0-9
    ///     $ - symbol
    ///     (space) - space character
    /// Examples:
    ///     "i w w 000$" => "Medium test phrase 123!"
    ///     "ii00$" => "TestPhrase11#"
    ///     "zzzzzz" => "t7Eq#r"
    /// </summary>
    /// <exception cref="ArgumentException">Throws if the argument is empty or contains invalid characters.</exception>
    /// <param name="s"></param>
    /// <returns>Passphrase object</returns>
    public Passphrase Parse(string s)
    {
        if (s.Length == 0)
        {
            throw new ArgumentException("Specification is empty.");
        }

        var rand = new Random();
        var tokenSequence = new List<ITokenSource>();

        foreach(var c in s)
        {
            ITokenSource tok = c switch
            {
                'w' => new TokenWord(_words, w => w.ToLower()),
                'u' => new TokenWord(_words, w => w.ToUpper()),
                'i' => new TokenWord(_words, w => w[..1].ToUpper() + w[1..].ToLower()),
                'r' => new TokenWord(_words, w =>
                {
                    var sb = new StringBuilder(w.ToLower());
                    var idx = rand.Next(sb.Length);
                    sb[idx] = char.ToUpper(sb[idx]);
                    return sb.ToString();
                }),
                'x' => new TokenChar(new string[] { TokenChar.Numerals, TokenChar.LowerAsciiLetters, TokenChar.UpperAsciiLetters }),
                'z' => new TokenChar(new string[] { TokenChar.Numerals, TokenChar.LowerAsciiLetters, TokenChar.UpperAsciiLetters, _symbols.Charset }),
                '$' => _symbols,
                '0' => new TokenNumber(1),
                ' ' => new TokenSpace(),
                _ => throw new ArgumentException("Invalid character in specification.")
            };

            tokenSequence.Add(tok);
        }

        return new Passphrase(tokenSequence);
    }
}
