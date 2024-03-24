using System;

namespace MPassDemo.Models;

public class PassphraseFieldModel
{
    public string SpecString { get; private set; }
    public string SpecError { get; private set; }
    public string Passphrase { get; private set; }
    public string Label { get; private set; }

    private readonly PassphraseSpecModel _specModel;

    public PassphraseFieldModel(string label, string specString, PassphraseSpecModel specModel)
    {
        Label = label;
        SpecString = specString;
        _specModel = specModel;

        Regenerate();
    }

    public void Regenerate()
    {
        try
        {
            Passphrase = _specModel.ParseAndGet(SpecString);
            SpecError = "";
        }
        catch (ArgumentException e)
        {
            Passphrase = "";
            SpecError = e.Message;
        }
    }
}
