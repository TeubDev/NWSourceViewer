namespace NWSourceViewer.Models.Classes.AlignmentRestrictions;

/// <summary>
/// Represents the silly AlignRstrctType column. If Neutral is in the alignment restriction, this indicates which neutral is forbidden.
/// </summary>
[Flags]
public enum AlignmentRestrictionTypes
{
    /// <summary>
    /// No restrictions in one direction or the other.
    /// </summary>
    None = 0,
    /// <summary>
    /// Neutral good, true neutral, or neutral evil.
    /// </summary>
    GoodEvil = 1,
    /// <summary>
    /// Lawful neutral, true neutral, or chaotic neutral.
    /// </summary>
    LawfulChaotic = 2
}
