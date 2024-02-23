namespace MPassTests;

public class PassphraseTests
{
    private string[] _testChars;
    private string[] _testWords;


    [SetUp]
    public void Setup()
    {
        _testChars = new string[] { "#", "!", "$" };
        _testWords = new string[] { "short", "list", "of", "very", "few", "words" };
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

        var phrase = string.Empty;
        var passphrase = new Passphrase(phraseSequence);
        for (var round = 0; round < 100; ++round)
        {
            phrase = passphrase.GetPassphrase();
            var elements = phrase.Split(" ");

            Assert.DoesNotThrow(() => int.Parse(elements[0]));
            Assert.That(_testChars, Does.Contain(elements[1]));
            Assert.That(_testWords, Does.Contain(elements[2]));
        }

        TestContext.WriteLine($"Last generated test passphrase: {phrase}");
    }

    [Test]
    public void TestTransform()
    {
        var phraseSequence = new List<ITokenSource> { new TokenWord(_testWords, w => w.ToUpper()) };
        var upperTestWords = _testWords.Select(w => w.ToUpper()).ToArray();

        var passphrase = new Passphrase(phraseSequence);
        for (var round = 0; round < 100; ++round)
        {
            Assert.That(upperTestWords, Does.Contain(passphrase.GetPassphrase()));
        }
    }

    [Test]
    public void TestSequenceParse()
    {
        var sequence = "i w w 000 $";

        var specifier = new Specifier(new TokenWord(_testWords), new TokenChar(_testChars));

        var passphrase = specifier.Parse(sequence);

        for(var round = 0; round < 100; ++round)
        {
            var p = passphrase.GetPassphrase();
            var toks = p.Split(' ');
            Assert.That(toks.Length, Is.EqualTo(5));

            Assert.That(char.IsUpper(toks[0][0]), Is.True);
            Assert.That(char.IsLower(toks[0][1]), Is.True);

            Assert.That(char.IsLower(toks[1][0]), Is.True);
            Assert.That(char.IsLower(toks[1][1]), Is.True);

            Assert.That(char.IsLower(toks[2][0]), Is.True);
            Assert.That(char.IsLower(toks[2][1]), Is.True);

            Assert.That(char.IsNumber(toks[3][0]), Is.True);
            Assert.That(char.IsNumber(toks[3][1]), Is.True);
            Assert.That(char.IsNumber(toks[3][1]), Is.True);

            Assert.That(_testChars, Does.Contain(toks[4][0].ToString()));
        }
    }
}
