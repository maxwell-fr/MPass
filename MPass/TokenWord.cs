using System.Collections;

namespace MPass;

public class TokenWord : ITokenSource
{
    private string[] _wordlist = new string[] { };
    private Func<string, string> _transform;

    public Random Random { private get; set; } = new Random();

    public TokenWord(IEnumerable<string> wordList) : this(wordList, t => t)
    {
    }

    public TokenWord(TokenWord tw) : this(tw._wordlist)
    {
    }

    public TokenWord(TokenWord tw, Func<string, string> transform) : this(tw._wordlist, transform)
    {
    }

    public TokenWord(IEnumerable<string> wordList, Func<string, string> transform)
    {
        _wordlist = wordList.ToArray();
        _transform = transform;

        if (_wordlist.Length < 1)
        {
            _wordlist = new[] { "tacos, burgers, salads" };
            throw new ArgumentException("No words in wordlist.");
        }
    }

    public string GetToken()
    {
         var idx = Random.Next(_wordlist.Length);
         return _transform(_wordlist[idx]);
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
