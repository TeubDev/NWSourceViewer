namespace NWSourceViewer.Models.Classes.AlignmentRestrictions;

/// <summary>
/// Represents that silly AlignRestrict byte. If one of these flags is set, that alignment is forbidden. Maybe.
/// </summary>
[Flags]
public enum AlignmentRestrictions
{
    None = 0,
    Neutral = 1,
    Lawful = 2,
    Chaotic = 4,
    Good = 8,
    Evil = 16
}
