using System.Globalization;

namespace NWSourceViewer.Models.Spells;

public class SpellModel : Base2daRowModel
{
    public override void ConvertData(Dictionary<string, string> data, TlkDictionary tlk)
    {
        Index = data["Index"].ToUint();
        var nameDataValue = data["Name"];
        var metamagicDataValue = data["MetaMagic"];
        if (nameDataValue != Constants.NullString && metamagicDataValue != Constants.NullString)
        {
            HasData = true;
            Name = nameDataValue.ToUint();
            NameString = tlk[Name];
            School = (SpellSchool)data["School"][0];
            Range = (SpellRangeType)data["Range"][0];
            HasVerbalComponents = data["VS"].Contains('V', StringComparison.InvariantCultureIgnoreCase);
            HasSomaticComponents = data["VS"].Contains('S', StringComparison.InvariantCultureIgnoreCase);
            Metamagic = (Metamagic)metamagicDataValue.To<int>();
            TargetType = (TargetType)data["TargetType"].To<int>();
            InnateLevel = data["Innate"].ToNUint();
            Master = data["Master"].ToNUint();
            SpellDesc = data["SpellDesc"].ToUint();
            DescriptionString = tlk[SpellDesc];
            SpontaneouslyCast = data["SpontaneouslyCast"].ToBool();
            IsHostile = data["HostileSetting"].ToBool();
            FeatID = data["FeatID"].ToNUint();
            SubradialSpells = data.GetUints("SubRadSpell1", "SubRadSpell2", "SubRadSpell3", "SubRadSpell4", "SubRadSpell5", "SubRadSpell6", "SubRadSpell7", "SubRadSpell8");
            Counters = data.GetUints("Counter1", "Counter2");
            ColumnData = data;
            UseableForPlayers = true;
        }
    }

    /// <summary>
    /// Can be an entry in dialog.TLK or a custom TLK, shows up in game as the spell title, and in feedback messages such as when casting unless overridden by AltMessage column.
    /// </summary>
    public uint Name { get; set; } = 0;

    /// <summary>
    /// Spell school to which the spell belongs.
    /// </summary>
    public SpellSchool School { get; set; }

    /// <summary>
    /// Range for how far a spell reaches.
    /// </summary>
    public SpellRangeType Range { get; set; }

    /// <summary>
    /// Verbal needs the caster to be able to speak (Eg; not silenced, deafened imposes 20% failure check).
    /// </summary>
    public bool HasVerbalComponents { get; set; }

    /// <summary>
    /// Somatic requires them to be able to use their arms (eg; not being Paralyzed) and is affected by armour failure chance.
    /// </summary>
    public bool HasSomaticComponents { get; set; }

    /// <summary>
    /// Indicates which metamagic feats can be used with the spell.
    /// </summary>
    public Metamagic Metamagic { get; set; }

    /// <summary>
    /// What types of targets are valid for the spell.
    /// </summary>
    public TargetType TargetType { get; set; }

    /// <summary>
    /// Innate level of the spell.
    /// </summary>
    public uint? InnateLevel { get; set; }

    /// <summary>
    /// The master of this spell if it is a subradial spell.
    /// </summary>
    public uint? Master { get; set; }

    /// <summary>
    /// Can be an entry in dialog.TLK or a custom TLK, shows up in game
    /// </summary>
    public uint SpellDesc { get; set; }

    /// <summary>
    /// If the caster has the "Can cast spontaneous spells" and this is true, then they can cast it and lose a different spell that is memorised of the same level.
    /// </summary>
    public bool SpontaneouslyCast { get; set; }

    /// <summary>
    /// Determines whether a spell is considered hostile when being cast on other creatures.
    /// </summary>
    public bool IsHostile { get; set; }

    /// <summary>
    /// Feat ID reference. Must be filled in for spells.2da lines attached to feats. There is a 1:1 mapping.
    /// </summary>
    public uint? FeatID { get; set; }

    /// <summary>
    /// Refers to the indexes of a spells available as a subradial option to this spell.
    /// </summary>
    public List<uint> SubradialSpells { get; set; } = new List<uint>();

    /// <summary>
    /// Refers to the indexes of a spells that this one can counter.
    /// </summary>
    public List<uint> Counters { get; set; } = new List<uint>();

    /// <summary>
    /// Gets the level a class can cast the spell at. If the class cannot cast the spell, returns null.
    /// </summary>
    /// <param name="classColumnName">Refers to the value in the "SpellTableColumn" column in classes.2da</param>
    public uint? GetLevelForClass(string classColumnName)
    {
        return ColumnData.GetNUint(classColumnName);
    }

    /// <summary>
    /// Since column titles can vary based on custom content, we can't strongly-type what each will be and need to store all the extra data here.
    /// </summary>
    private Dictionary<string, string> ColumnData = new Dictionary<string, string>();
}
