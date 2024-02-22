namespace MPassTests;

public class TokenTests
{
    private string[] _testChars;
    private string[] _testWords;


    [SetUp]
    public void Setup()
    {
        _testChars = new string[] { TokenChar.AsciiSymbols, TokenChar.Numerals };
        _testWords = new string[] { "short", "list", "of", "a", "few", "words" };
    }

    [Test]
    public void TestTokenChar()
    {
        var tc = new TokenChar(_testChars);

        var testCharStr = string.Concat(_testChars);
        for (var round = 0; round < 100; ++round)
        {
            Assert.That(testCharStr.Contains(tc.GetToken()), Is.True);
        }
    }

    [Test]
    public void TestTokenWord()
    {
        var tw = new TokenWord(_testWords);

        for (var round = 0; round < 100; ++round)
        {
            Assert.That(_testWords, Does.Contain(tw.GetToken()));
        }
    }

    [Test]
    public void TestTokenSpace()
    {
        var ts = new TokenSpace();

        Assert.That(ts.GetToken(), Is.EqualTo(" "));
    }

    [Test]
    public void TestTokenNumber()
    {
        var tn = new TokenNumber(8);

        for (var round = 0; round < 100; ++round)
        {
            var tok = tn.GetToken();
            Assert.That(tok.Length, Is.EqualTo(8));
            Assert.DoesNotThrow(() => int.Parse(tok));
        }
    }
}
