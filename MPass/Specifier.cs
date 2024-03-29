﻿using System.Text;

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
    ///     w - lowercase word (example)
    ///     W - uppercase word (EXAMPLE)
    ///     i - initial caps word (Example)
    ///     r - random cap word (exAmple)
    ///     a - random lowercase letter
    ///     A - random uppercase letter
    ///     x - random single alphanumeric character
    ///     z - random single alphanumeric or symbol character (combines x and $)
    ///     # - digit, 0-9
    ///     $ - symbol
    ///     (space) - space character
    ///     ? - shuffle the sequence (if present, the token order will be randomized)
    /// Examples:
    ///     "i w w ###$" => "Medium test phrase 123!"
    ///     "ii##$" => "TestPhrase11#"
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
        bool shuffle = false;

        foreach(var c in s)
        {
            if(c == '?')
            {
                shuffle = true;
                continue;
            }
            ITokenSource tok = c switch
            {
                'w' => new TokenWord(_words, w => w.ToLower()),
                'W' => new TokenWord(_words, w => w.ToUpper()),
                'i' => new TokenWord(_words, w => w[..1].ToUpper() + w[1..].ToLower()),
                'r' => new TokenWord(_words, w =>
                {
                    var sb = new StringBuilder(w.ToLower());
                    var idx = rand.Next(sb.Length);
                    sb[idx] = char.ToUpper(sb[idx]);
                    return sb.ToString();
                }),
                'a' => new TokenChar(TokenChar.LowerAsciiLetters),
                'A' => new TokenChar(TokenChar.UpperAsciiLetters),
                'x' => new TokenChar(new string[] { TokenChar.Numerals, TokenChar.LowerAsciiLetters, TokenChar.UpperAsciiLetters }),
                'z' => new TokenChar(new string[] { TokenChar.Numerals, TokenChar.LowerAsciiLetters, TokenChar.UpperAsciiLetters, _symbols.Charset }),
                '$' => _symbols,
                '#' => new TokenNumber(1),
                ' ' => new TokenSpace(),
                _ => throw new ArgumentException("Invalid character in specification.")
            };

            tokenSequence.Add(tok);
        }

        return new Passphrase(tokenSequence, shuffle);
    }
}
