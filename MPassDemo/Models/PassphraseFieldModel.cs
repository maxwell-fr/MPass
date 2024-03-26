using System;

namespace MPassDemo.Models;
/// <summary>
/// Represents the passphrase field display itself.
/// </summary>
public class PassphraseFieldModel
{
    public string SpecString { get; private set; }
    public string SpecError { get; private set; }
    public string Passphrase { get; private set; }
    public string Label { get; private set; }

    private readonly PassphraseSpecModel _specModel;

    /// <summary>
    /// Create a new FieldModel.
    /// </summary>
    /// <param name="label">The label text associated with it.</param>
    /// <param name="specString">The spec string to be used for passphrase generation.</param>
    /// <param name="specModel">The underlying specification model.</param>
    public PassphraseFieldModel(string label, string specString, PassphraseSpecModel specModel)
    {
        Label = label;
        SpecString = specString;
        _specModel = specModel;

        Regenerate();
    }

    /// <summary>
    /// Regenerate the passphrase. This sets Passphrase and SpecError, as needed.
    /// </summary>
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
