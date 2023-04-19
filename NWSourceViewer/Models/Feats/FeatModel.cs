namespace NWSourceViewer.Models.Feats;

public class FeatModel : Base2daRowModel
{
    public override void ConvertData(Dictionary<string, string> data, TlkDictionary tlk)
    {
        Index = data["Index"].ToUint();
        Feat = data["FEAT"].ToNUint();
        if (Feat != null)
        {
            HasData = true;
            NameString = tlk[(uint)Feat];
            Description = data["DESCRIPTION"].ToNUint();
            MinAttackBonus = data["MINATTACKBONUS"].ToNUint();
            MinStr = data["MINSTR"].ToNUint();
            MinDex = data["MINDEX"].ToNUint();
            MinInt = data["MININT"].ToNUint();
            MinWis = data["MINWIS"].ToNUint();
            MinCon = data["MINCON"].ToNUint();
            MinCha = data["MINCHA"].ToNUint();
            MinSpellLvl = data["MINSPELLLVL"].ToNUint();
            PrereqFeat1 = data["PREREQFEAT1"].ToNUint();
            PrereqFeat2 = data["PREREQFEAT2"].ToNUint();
            AllClassesCanUse = data["ALLCLASSESCANUSE"].ToBool();
            SpellId = data["SPELLID"].ToNUint();
            Successor = data["SUCCESSOR"].ToNUint();
            UsesPerDay = data["USESPERDAY"].ToNInt();
            TargetSelf = data["TARGETSELF"].ToNBool();
            OrReqFeat0 = data["OrReqFeat0"].ToNUint();
            OrReqFeat1 = data["OrReqFeat1"].ToNUint();
            OrReqFeat2 = data["OrReqFeat2"].ToNUint();
            OrReqFeat3 = data["OrReqFeat3"].ToNUint();
            OrReqFeat4 = data["OrReqFeat4"].ToNUint();
            ReqSkill = data["REQSKILL"].ToNUint();
            ReqSkillMinRanks = data["ReqSkillMinRanks"].ToNUint();
            ReqSkill2 = data["REQSKILL"].ToNUint();
            ReqSkillMinRanks2 = data["ReqSkillMinRanks"].ToNUint();
            HostileFeat = data["HostileFeat"].ToBool();
            MinLevel = data["MinLevel"].ToNUint();
            MinLevelClass = data["MinLevelClass"].ToNUint();
            MaxLevel = data["MaxLevel"].ToNUint();
            MinFortSave = data["MinFortSave"].ToNUint();
            PreReqEpic = data["PreReqEpic"].ToBool();
            ReqAction = data["ReqAction"].ToBool();
        }
    }

    /// <summary>
    /// A StringRef indicating the name of the feat.
    /// </summary>
    public uint? Feat { get; set; } = null;
    /// <summary>
    /// A StringRef indicating the description of the feat.
    /// </summary>
    public uint? Description { get; set; } = null;
    /// <summary>
    /// The minimum attack bonus a character must have to select this feat.
    /// </summary>
    public uint? MinAttackBonus { get; set; } = null;
    /// <summary>
    /// The minimum strength ability score a character must have to select this feat.
    /// </summary>
    public uint? MinStr { get; set; } = null;
    /// <summary>
    /// The minimum dexterity ability score a character must have to select this feat.
    /// </summary>
    public uint? MinDex { get; set; } = null;
    /// <summary>
    /// The minimum intelligence ability score a character must have to select this feat.
    /// </summary>
    public uint? MinInt { get; set; } = null;
    /// <summary>
    /// The minimum wisdom ability score a character must have to select this feat.
    /// </summary>
    public uint? MinWis { get; set; } = null;
    /// <summary>
    /// The minimum constitution ability score a character must have to select this feat.
    /// </summary>
    public uint? MinCon { get; set; } = null;
    /// <summary>
    /// The minimum charisma ability score a character must have to select this feat.
    /// </summary>
    public uint? MinCha { get; set; } = null;
    /// <summary>
    /// The minimum spell level a spellcasting character must be able to cast in order to select this feat.
    /// </summary>
    public uint? MinSpellLvl { get; set; } = null;
    /// <summary>
    /// If this feat requires another feat to be selected before this one may be chosen, the ID number of the required feat(s) will be in these columns. If both are defined then both are required.
    /// </summary>
    public uint? PrereqFeat1 { get; set; } = null;
    /// <summary>
    /// If this feat requires another feat to be selected before this one may be chosen, the ID number of the required feat(s) will be in these columns. If both are defined then both are required.
    /// </summary>
    public uint? PrereqFeat2 { get; set; } = null;
    /// <summary>
    /// Value of 0 (false) or 1 (true). Determines whether all classes can use this feat or not. If 0 (not), the appropriate class feat 2da files (cls_feat_*) determine whether or not this feat is available to the character.
    /// </summary>
    public bool AllClassesCanUse { get; set; } = false;
    /// <summary>
    /// The ID number in spells.2da for the equivalent spell script to run for this feat, if it needs one.
    /// </summary>
    public uint? SpellId { get; set; } = null;
    /// <summary>
    /// The ID number of the feat which follows this feat. For example, the Disarm feat has the ID number of Improved Disarm in this column.
    /// </summary>
    public uint? Successor { get; set; } = null;
    /// <summary>
    /// Number of uses per day.
    /// </summary>
    /// <remarks>
    /// -1 if uses per day depends on certain hardcoded conditions such as number of levels in a class (Example, stunning fist).
    /// **** (null) if feat can be used unlimited times per day or if feat is passive.
    /// Else it is the number of times per day this feat may be used which can be 1 - 99. 100 or more gets acted on as if it is unlimited.
    /// </remarks>
    public int? UsesPerDay { get; set; } = null;
    /// <summary>
    /// Value 0 (false) or 1 (true). **** if not usable.
    /// </summary>
    public bool? TargetSelf { get; set; } = null;
    /// <summary>
    /// Similar to the PREREQFEAT columns above, this is the ID of a required feat the character needs to select this feat. These columns are different in that the character is only required to have one of the feats specified in these columns.
    /// </summary>
    public uint? OrReqFeat0 { get; set; } = null;
    /// <summary>
    /// Similar to the PREREQFEAT columns above, this is the ID of a required feat the character needs to select this feat. These columns are different in that the character is only required to have one of the feats specified in these columns.
    /// </summary>
    public uint? OrReqFeat1 { get; set; } = null;
    /// <summary>
    /// Similar to the PREREQFEAT columns above, this is the ID of a required feat the character needs to select this feat. These columns are different in that the character is only required to have one of the feats specified in these columns.
    /// </summary>
    public uint? OrReqFeat2 { get; set; } = null;
    /// <summary>
    /// Similar to the PREREQFEAT columns above, this is the ID of a required feat the character needs to select this feat. These columns are different in that the character is only required to have one of the feats specified in these columns.
    /// </summary>
    public uint? OrReqFeat3 { get; set; } = null;
    /// <summary>
    /// Similar to the PREREQFEAT columns above, this is the ID of a required feat the character needs to select this feat. These columns are different in that the character is only required to have one of the feats specified in these columns.
    /// </summary>
    public uint? OrReqFeat4 { get; set; } = null;
    /// <summary>
    /// The ID value in skills.2da of a required skill the character must have to be able to select this feat.
    /// </summary>
    public uint? ReqSkill { get; set; } = null;
    /// <summary>
    /// The minimum number of ranks the character must have of REQSKILL to be able to select this feat. Ranks are what has been assigned at levelup not the bonuses from abilities, items or other sources.
    /// </summary>
    /// <remarks>
    /// If **** (null) then the skill itself must be learnable, stopping Skill Focus: Animal Empathy for instance for those classes unable to get ranks in that skill.
    /// </remarks>
    public uint? ReqSkillMinRanks { get; set; } = null;
    /// <summary>
    /// The ID value in skills.2da of a required skill the character must have to be able to select this feat.
    /// </summary>
    public uint? ReqSkill2 { get; set; } = null;
    /// <summary>
    /// The minimum number of ranks the character must have of REQSKILL2 to be able to select this feat. Ranks are what has been assigned at levelup not the bonuses from abilities, items or other sources.
    /// </summary>
    /// <remarks>
    /// If **** (null) then the skill itself must be learnable, stopping Skill Focus: Animal Empathy for instance for those classes unable to get ranks in that skill.
    /// </remarks>
    public uint? ReqSkillMinRanks2 { get; set; } = null;
    /// <summary>
    /// Value 0 (false) or 1 (true). Determines whether the use of this feat is considered as a hostile act.
    /// </summary>
    public bool HostileFeat { get; set; } = false;
    /// <summary>
    /// The minimum level a character must have to be able to take this feat. Works with MinLevelClass.
    /// </summary>
    public uint? MinLevel { get; set; } = null;
    /// <summary>
    /// The ID value of the corresponding class from classes.2da the character must have MinLevel levels in.
    /// </summary>
    public uint? MinLevelClass { get; set; } = null;
    /// <summary>
    /// The maximum character level allowed a character to be able to select this feat.
    /// </summary>
    public uint? MaxLevel { get; set; } = null;
    /// <summary>
    /// The minimum fortitude saving throw bonus a character must have to be able to select this feat.
    /// </summary>
    public uint? MinFortSave { get; set; } = null;
    /// <summary>
    /// Indicates if only epic characters can choose this feat. A value of 1 indicates epic-only, while 0 indicates no such requirement.
    /// </summary>
    public bool PreReqEpic { get; set; } = false;
    /// <summary>
    /// 0 = This is an instant feat, bypassing the action queue and without animation. If linked to a spells.2da line the ImpactScript will fire immediately.
    /// 1 = Feat activation uses the action queue and plays an animation.
    /// </summary>
    public bool ReqAction { get; set; } = false;
}
