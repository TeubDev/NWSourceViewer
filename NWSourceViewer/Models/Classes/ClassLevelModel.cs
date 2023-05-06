using NWSourceViewer.Models.Feats;

namespace NWSourceViewer.Models.Classes;

/// <summary>
/// Represents the data about a class at a particular level.
/// </summary>
public class ClassLevelModel
{
    public uint Level { get; set; }
    public uint BaseAttackBonus { get; set; }
    public uint HitPointMinimum { get; set; }
    public uint HitPointMaximum { get; set; }
    public uint FortitudeSave { get; set; }
    public uint ReflexSave { get; set; }
    public uint WillSave { get; set; }
    public uint BonusFeatCount { get; set; }
    public List<FeatModel> GeneralFeats { get; set; } = new List<FeatModel>();
    public List<FeatModel> GeneralOrBonusFeats { get; set; } = new List<FeatModel>();
    public List<FeatModel> BonusFeats { get; set; } = new List<FeatModel>();
    public List<FeatModel> AutomaticFeats { get; set; } = new List<FeatModel>();

    /// <summary>
    /// Indicates how many spells of which levels the class knows at this class level. Null means no known spells.
    /// </summary>
    public ClassLevelSpells? SpellsKnown { get; set; }

    /// <summary>
    /// Indicates the number of slots of which levels the class has at this class level. Null means no spell slots.
    /// </summary>
    public ClassLevelSpells? SpellsGained { get; set; }
}
