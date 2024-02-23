using System.Collections;

namespace MPass;

/// <summary>
/// A token source that returns only a single space character.
/// </summary>
public class TokenSpace : TokenChar
{

    public TokenSpace() : base(new[]{" "}) {
    }

}
