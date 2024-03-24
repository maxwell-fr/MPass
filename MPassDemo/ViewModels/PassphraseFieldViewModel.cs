using MPassDemo.Models;
using ReactiveUI;

namespace MPassDemo.ViewModels;

public class PassphraseFieldViewModel : ViewModelBase
{
    private readonly PassphraseFieldModel _passphraseFieldModel;

    public PassphraseFieldViewModel(PassphraseFieldModel fieldModel)
    {
        _passphraseFieldModel = fieldModel;
        RegeneratePassphrase();
    }

    public string Label => _passphraseFieldModel.Label;
    public string SpecString => _passphraseFieldModel.SpecString;
    public bool IsError {
        get; private set;
    }

    private string _passphraseResult;
    public string PassphraseResult
    {
        get => _passphraseResult;
        set => this.RaiseAndSetIfChanged(ref _passphraseResult, value);
    }

    public void RegeneratePassphrase()
    {
        _passphraseFieldModel.Regenerate();
        if (_passphraseFieldModel.SpecError == "")
        {
            PassphraseResult = _passphraseFieldModel.Passphrase;
            IsError = false;
        }
        else
        {
            PassphraseResult = _passphraseFieldModel.SpecError;
            IsError = true;
        }
    }
}
