namespace MPassTests;

public class PassphraseTests
{
    private string[] _testChars;
    private string[] _testWords;


    [SetUp]
    public void Setup()
    {
        _testChars = new string[] { "#", "!", "$" };
        _testWords = new string[] { "short", "list", "of", "a", "few", "words" };
    }

    [Test]
    public void TestSimplePassphrase()
    {
        var phraseSequence = new List<ITokenSource>
        {
            new TokenNumber(6),
            new TokenSpace(),
            new TokenChar(_testChars),
            new TokenSpace(),
            new TokenWord(_testWords)
        };

        var phrase = new Passphrase(phraseSequence).GetPassphrase();
        var elements = phrase.Split(" ");

        Assert.DoesNotThrow(() => int.Parse(elements[0]));
        Assert.That(_testChars, Does.Contain(elements[1]));
        Assert.That(_testWords, Does.Contain(elements[2]));

        TestContext.WriteLine($"Generated test passphrase: {phrase}");
    }
}
