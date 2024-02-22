using System.Collections;

namespace MPass;

public class TokenWord : ITokenSource
{
    private string[] _wordlist = new string[] { };

    public Random Random { private get; set; } = new Random();

    public TokenWord(IEnumerable<string> wordList)
    {
        _wordlist = wordList.ToArray();
        if (_wordlist.Length < 1)
        {
            _wordlist = new[] { "tacos, burgers, salads" };
            throw new ArgumentException("No words in wordlist.");
        }
    }

    public string GetToken()
    {
         var idx = Random.Next(_wordlist.Length);
         return _wordlist[idx].ToString();
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
