using NWSourceViewer.Models.Classes.AlignmentRestrictions;

namespace NWSourceViewer.Models.Classes;

public class ClassModel : Base2daRowModel
{

    public override void ConvertData(Dictionary<string, string> data, TlkDictionary tlk)
    {
        Index = data["Index"].ToUint();
        var nameDataValue = data["Name"];
        if (nameDataValue != Constants.NullString)
        {
            HasData = true;
            Name = nameDataValue.ToUint();
            NameString = tlk[Name];
            Description = data["Description"].ToUint();
            DescriptionString = tlk[Description];
            HitDie = data["HitDie"].ToUint();
            AttackBonusTable = data["AttackBonusTable"];
            FeatsTable = data["FeatsTable"];
            SavingThrowTable = data["SavingThrowTable"];
            SkillsTable = data["SkillsTable"];
            BonusFeatsTable = data["BonusFeatsTable"];
            SkillPointBase = data["SkillPointBase"].ToUint();
            SpellGainTable = data["SpellGainTable"];
            SpellKnownTable = data["SpellKnownTable"];
            PlayerClass = data["PlayerClass"].ToBool();
            SpellCaster = data["SpellCaster"].ToBool();
            AlignRestrict = (AlignmentRestrictions.AlignmentRestrictions)data["AlignRestrict"].To<byte>();
            AlignRstrctType = (AlignmentRestrictionTypes)data["AlignRstrctType"].To<byte>();
            InvertRestrict = data["InvertRestrict"].ToBool();
            PreReqTable = data["PreReqTable"];
            MaxLevel = data["MaxLevel"].ToUint();
            XPPenalty = data["XPPenalty"].ToBool();
            ArcSpellLvlMod = data["ArcSpellLvlMod"].ToUint();
            DivSpellLvlMod = data["DivSpellLvlMod"].ToUint();
            EpicLevel = data["EpicLevel"].ToInt();
            StatGainTable = data["StatGainTable"];
            MemorizesSpells = data["MemorizesSpells"].ToBool();
            SpellbookRestricted = data["SpellbookRestricted"].ToBool();
            PickDomains = data["PickDomains"].ToBool();
            PickSchool = data["PickSchool"].ToBool();
            LearnScroll = data["LearnScroll"].ToBool();
            Arcane = data["Arcane"].ToBool();
            ASF = data["ASF"].ToBool();
            SpellcastingAbil = data["SpellcastingAbil"];
            SpellTableColumn = data["SpellTableColumn"];
            CLMultiplier = data["CLMultiplier"].ToFloat();
            MinCastingLevel = data["MinCastingLevel"].ToUint();
            MinAssociateLevel = data["MinAssociateLevel"].ToUint();
            CanCastSpontaneously = data["CanCastSpontaneously"].ToBool();
            UseableForPlayers = PlayerClass;
        }
    }

    /// <summary>
    /// A StringRef indicating a string that will be displayed in the game as the name of the class ("Bard", "Barbarian") etc.)
    /// </summary>
    public uint Name { get; set; } = 0;
    /// <summary>
    /// A StringRef indicating a string that will be displayed in the game as the class description.
    /// </summary>
    public uint Description { get; set; } = 0;
    /// <summary>
    /// This determines what size of die is used to roll hit points on level-up. (4, 6, 8, 10, 12, etc.)
    /// </summary>
    public uint HitDie { get; set; } = 0;
    /// <summary>
    /// The name of the .2da file (without the extension) to be consulted for the base attack bonus progression of this class.
    /// </summary>
    public string AttackBonusTable { get; set; } = "";
    /// <summary>
    /// The name of the .2da file (without the extension) defining the class feats.
    /// </summary>
    public string FeatsTable { get; set; } = "";
    /// <summary>
    /// The name of the .2da file (without the extension) defining the saving throw progression of this class at pre-epic levels.
    /// </summary>
    public string SavingThrowTable { get; set; } = "";
    /// <summary>
    /// The name of the .2da file (without the extension) defining the skills available to this class, including specifying which ones are class skills.
    /// </summary>
    public string SkillsTable { get; set; } = "";
    /// <summary>
    /// The name of the .2da file (without the extension) defining the class levels at which this class receives bonus feats.
    /// </summary>
    public string BonusFeatsTable { get; set; } = "";
    /// <summary>
    /// This is number of skill points gained per level (before the intelligence modifier and before quadrupling at character level 1).
    /// </summary>
    public uint SkillPointBase { get; set; } = 0;
    /// <summary>
    /// The name of the .2da file (without the extension) defining how many spell slots this class has at each level.
    /// </summary>
    public string SpellGainTable { get; set; } = "";
    /// <summary>
    /// The name of the .2da file (without the extension) defining how many spells are known by this class at each level (for classes with a limited number of known spells, specifically bards and sorcerers).
    /// </summary>
    public string SpellKnownTable { get; set; } = "";
    /// <summary>
    /// Controls whether or not this class is available for players in the game, else if 0 it will restrict it to NPC classes in the toolset. If setting the vanilla classes to 0, they will be permanently grayed out, whereas for custom content, this setting determines if it shows up in the class list. If you want to hide the vanilla classes from showing up, you'd need to make sure this is 0 and that the Name column is set to a non-default value.
    /// </summary>
    public bool PlayerClass { get; set; } = false;
    /// <summary>
    /// Leave as 0 for custom classes that add to existing spell casting classes. If you set to 1, this will prevent the DivSpellLvlMod and ArcSpellLvlMod from working.
    /// Set to 1 for a custom class with its own spell list, as with the new NWN:EE columns below.
    /// </summary>
    public bool SpellCaster { get; set; } = false;
    /// <summary>
    /// Specifies the alignments that are not allowed to obtain levels in this class.
    /// </summary>
    public AlignmentRestrictions.AlignmentRestrictions AlignRestrict { get; set; }
    /// <summary>
    /// Specifies if the alignment restriction specified in the AlignRestrict column applies to the law-chaos axis (0x1), the good-evil axis (0x2), both (0x3), or neither (0x0).
    /// </summary>
    public AlignmentRestrictionTypes AlignRstrctType { get; set; }
    /// <summary>
    /// If set to 1 (the default is 0), the alignments specified in the AlignRestrict and AlignRestrictType columns are the only alignments allowed to gain levels in this class, rather than being the alignments prohibited from gaining them.
    /// </summary>
    public bool InvertRestrict { get; set; }
    /// <summary>
    /// The name of the .2da file (without the extension) defining the prerequisites for this class (making this class a prestige class).
    /// </summary>
    public string PreReqTable { get; set; } = "";
    /// <summary>
    /// The maximum level of this class that can be taken, or 0 for no limit. Usually only applies to prestige classes (although most are set to 40).
    /// </summary>
    public uint MaxLevel { get; set; } = 0;
    /// <summary>
    /// Determines if this class is counted when determining if a multiclass penalty applies. 1 = counted (normal for base classes); 0 = not counted (normal for prestige classes).
    /// </summary>
    public bool XPPenalty { get; set; } = false;
    /// <summary>
    /// If positive, this specifies the number of levels in this class that together add one level to an arcane class when determining the spell slots based on class level.
    /// </summary>
    public uint ArcSpellLvlMod { get; set; } = 0;
    /// <summary>
    /// If positive, this specifies the number of levels in this class that together add one level to a divine class when determining the spell slots based on class level. That is, the number of levels in this class are divided by this number, with fractions of half or more rounded up. The result is added to a divine class level before the spell slots for that class are determined.
    /// </summary>
    public uint DivSpellLvlMod { get; set; } = 0;
    /// <summary>
    /// The number of levels of this class that can be taken pre-epic (before character level 21; c.f. prestige class). A value of -1 indicates that there is no limit.
    /// </summary>
    public int EpicLevel { get; set; } = 0;
    /// <summary>
    /// The name of the .2da file (without the extension) defining the ability and natural AC gains for this class at each level.
    /// </summary>
    public string StatGainTable { get; set; } = "";
    /// <summary>
    /// When set to 1 (including when Spellcaster is 1), indicates this class gains spells by memorization (e.g. wizards).
    /// </summary>
    public bool MemorizesSpells { get; set; } = false;
    /// <summary>
    /// When set to 1 (including when Spellcaster is 1), indicates that the spell caster is restricted to selecting spells from a spellbook (e.g. wizards).
    /// </summary>
    public bool SpellbookRestricted { get; set; } = false;
    /// <summary>
    /// When set to 1 (including when Spellcaster is 1), indicates that the spell caster can choose clerical domains as part of their class
    /// </summary>
    public bool PickDomains { get; set; } = false;
    /// <summary>
    /// When set to 1 (including when Spellcaster is 1), indicates that the spell caster can choose a spell school as part of their class
    /// </summary>
    public bool PickSchool { get; set; } = false;
    /// <summary>
    /// When set to 1 (including when Spellcaster is 1), indicates that the spell caster can learn spells from reading scrolls (e.g. wizards)
    /// </summary>
    public bool LearnScroll { get; set; } = false;
    /// <summary>
    /// When set to 1, indicates that the spell caster uses arcane spells (otherwise they are divine spell casters)
    /// </summary>
    public bool Arcane { get; set; } = false;
    /// <summary>
    /// When set to 1, indicates that the spell caster is subject to the effects of arcane spell failure
    /// </summary>
    public bool ASF { get; set; } = false;
    /// <summary>
    /// Ability score used as the primary source of their magical powers
    /// </summary>
    public string SpellcastingAbil { get; set; } = "";
    /// <summary>
    /// Column in the spells.2da file indicating which level the spell is. Values can be: Bard,  Cleric, Druid, Paladin, Ranger and Wiz_Sorc
    /// </summary>
    public string SpellTableColumn { get; set; } = "";
    /// <summary>
    /// Caster Level multiplier
    /// </summary>
    public float CLMultiplier { get; set; } = 0;
    /// <summary>
    /// Minimum level required to cast spells, primarily used for Paladin and Ranger style classes.
    /// </summary>
    public uint MinCastingLevel { get; set; } = 0;
    /// <summary>
    /// 1 to enable associate (Familar for Arcane casters, Animal Companion for Divine casters). 255 to disable adding one (eg; Cleric). If you have something other than 1 to enable, it becomes a minimum level for the class to receive their Animal Companion and increase its level (Does not work for Familiars).
    /// </summary>
    public uint MinAssociateLevel { get; set; } = 0;
    /// <summary>
    /// Indicates that spells can be cast spontaneously. This specifically means casting spells marked as "Spontaneous" in spells.2da, not spontaneous casters like Sorcerers and Bards (who can pick any spell they know to cast).
    /// </summary>
    public bool CanCastSpontaneously { get; set; } = false;
}
