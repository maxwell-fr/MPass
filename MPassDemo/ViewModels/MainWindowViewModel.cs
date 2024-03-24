using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using DynamicData.Binding;
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
            UpdatePassphrase(value);
            this.RaiseAndSetIfChanged(ref _specString, value);
        }
    }

    private string _passphrase = string.Empty;

    public string Passphrase
    {
        get => _passphrase;
        set => this.RaiseAndSetIfChanged(ref _passphrase, value);
    }

    public void UpdatePassphrase(string spec)
    {
        try
        {
            Passphrase = _passphraseSpecModel.ParseAndGet(spec);
        }
        catch (ArgumentException e)
        {
            Passphrase = $"{e.Message}";
        }
    }

    public string Helptext =>
        """
        Use one or more of the following keys in the Specification:
        w - lowercase word (example)
        W - uppercase word (EXAMPLE)
        i - initial caps word (Example)
        r - random cap word (exAmple)
        a - random lowercase letter
        A - random uppercase letter
        x - random single alphanumeric character
        # - digit, 0-9
        $ - symbol
        z - combination of x and $
        (space) - space character
        ? - shuffle the sequence 
        Examples:
        i w w ###$ => Medium test phrase 123!
        ii##$ => TestPhrase11#
        zzzzzz => t7Eq#r

        """;

    public ReactiveCommand<Unit, Unit> OpenWordListFile { get; }
    public Interaction<Unit, IStorageFile?> FileDialog { get; }

    public ObservableCollection<PassphraseFieldViewModel> PassphraseFields { get; } = new();

    public MainWindowViewModel()
    {
        OpenWordListFile = ReactiveCommand.CreateFromTask(OpenFileAsync);
        FileDialog = new Interaction<Unit, IStorageFile?>();

        //Some test fields for demonstration
        PassphraseFields.Add(new PassphraseFieldViewModel(
            new PassphraseFieldModel("Test1", "W w w #####", new PassphraseSpecModel())
        ));
        PassphraseFields.Add(new PassphraseFieldViewModel(
            new PassphraseFieldModel("Test2", "w W w W", new PassphraseSpecModel())
        ));
        PassphraseFields.Add(new PassphraseFieldViewModel(
            new PassphraseFieldModel("Test3", "flarh", new PassphraseSpecModel())
        ));
        PassphraseFields.Add(new PassphraseFieldViewModel(
            new PassphraseFieldModel("Test4", "", new PassphraseSpecModel())
        ));
    }

    private async Task OpenFileAsync()
    {
            var file = await FileDialog.Handle(Unit.Default);

            if (file is null)
            {
                Console.WriteLine("File pick cancelled.");
                return;
            }
            Console.WriteLine($"file picked: {file.Name}");

            //todo: various error handling
            var reader = await file.OpenReadAsync();
            var length = (int)(reader.Length < int.MaxValue ? reader.Length : int.MaxValue);
            var buf = new byte[length];
            var numBytesRead = 0;

            while (numBytesRead < length)
            {
                int byteCount = await reader.ReadAsync(buf, numBytesRead, length - numBytesRead);
                //todo: check for errors, maybe a busy spinner, idk
                numBytesRead += byteCount;
            }

            var contents = new string(System.Text.Encoding.ASCII.GetString(buf));
            var newPassphrase = new PassphraseSpecModel(contents);
            _passphraseSpecModel = newPassphrase;
            UpdatePassphrase(_specString);
    }


}
