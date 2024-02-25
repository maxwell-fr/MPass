
namespace MPassDemo.Models;
using MPass;

public class PassphraseSpecModel
{
    private const string _defaultWordList = "the and you that was for are with his they one have this from had not word but what some can out other were all there when use your how said each she which their time will way about many then them write would like these her long make thing see him two has look more day could come did number sound most people over know water than call first who may down side been now find any new work";
    private Specifier _specifier;

    public PassphraseSpecModel() : this(_defaultWordList)
    {
    }

    public PassphraseSpecModel(string wordList)
    {
        var words = new TokenWord(wordList.Split(' ', '\n', '\r', '\t'));
        var symbols = new TokenChar(new[]{TokenChar.EzAsciiSymbols});
        _specifier = new Specifier(words, symbols);
    }

    public string ParseAndGet(string s)
    {
        return _specifier.Parse(s).GetPassphrase();
    }

}
