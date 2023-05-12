namespace NWSourceViewer.Models.Classes.AlignmentRestrictions;

/// <summary>
/// Represents which alignments are allowed for a class.
/// </summary>
public class AlignmentsAllowed
{
    public bool LawfulGood { get; set; }
    public bool NeutralGood { get; set; }
    public bool ChaoticGood { get; set; }
    public bool LawfulNeutral { get; set; }
    public bool TrueNeutral { get; set; }
    public bool ChaoticNeutral { get; set; }
    public bool LawfulEvil { get; set; }
    public bool NeutralEvil { get; set; }
    public bool ChaoticEvil { get; set; }
}
