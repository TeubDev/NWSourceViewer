namespace NWSourceViewer.Models.Spells;

/// <summary>
/// Indicates which metamagic feats can be applied to a spell.
/// </summary>
[Flags]
public enum Metamagic
{
    None = 0,
    Empower = 1,
    Extend = 2,
    Maximize = 4,
    Quicken = 8,
    Silent = 16,
    Still = 32,
}
