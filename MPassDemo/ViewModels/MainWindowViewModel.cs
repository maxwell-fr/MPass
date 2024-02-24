using System;
using MPassDemo.Models;
using ReactiveUI;

namespace MPassDemo.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private PassphraseSpecModel _passphraseSpecModel = new PassphraseSpecModel();
    private string _specString = string.Empty;

    public string SpecString
    {
        get => _specString;
        set
        {
            try
            {
                Passphrase = _passphraseSpecModel.ParseAndGet(value);
            }
            catch (ArgumentException e)
            {
                Passphrase = $"{e.Message}";
            }

            this.RaiseAndSetIfChanged(ref _specString, value);
        }
    }

    private string _passphrase = string.Empty;

    public string Passphrase
    {
        get => _passphrase;
        set => this.RaiseAndSetIfChanged(ref _passphrase, value);
    }

    public string Helptext =>
        """
        Use one or more of the following keys in the Specification:
        w - lowercase word (examnple)
        u - uppercase word (EXAMPLE)
        i - initial caps word (Example)
        r - random cap word (exAmple)
        x - random single alphanumeric character
        z - random single alphanumeric or symbol character
        0 - digit, 0-9
        $ - symbol
        (space) - space character
        Examples:
        i w w 000$ => Medium test phrase 123!
        ii00$ => TestPhrase11#
        zzzzzz => t7Eq#r

        """;

}
