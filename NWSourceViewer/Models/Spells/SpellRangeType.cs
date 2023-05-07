namespace NWSourceViewer.Models.Spells;

/// <summary>
/// Describes ranges.
/// </summary>
public enum SpellRangeType
{
    /// <summary>
    /// 0M (but seems to default to 2.25 if it is targeting another object, eg; a scroll on the ground to enchant)
    /// </summary>
    Personal = 'P',
    /// <summary>
    /// 2.25M
    /// </summary>
    Touch = 'T',
    /// <summary>
    /// 8M
    /// </summary>
    Short = 'S',
    /// <summary>
    /// 20M
    /// </summary>
    Medium = 'M',
    /// <summary>
    /// 40M
    /// </summary>
    Long = 'L'
}
