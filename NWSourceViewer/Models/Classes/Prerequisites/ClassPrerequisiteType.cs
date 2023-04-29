namespace NWSourceViewer.Models.Classes.Prerequisites;

/// <summary>
/// Defines what type of prerequisite this is.
/// </summary>
public enum ClassPrerequisiteType
{
    /// <summary>
    /// ReqParam1 is the required combined class levels which are arcane spellcasters  - ie is a spellcaster and has Arcane set to 1 in classes.2da
    /// </summary>
    ArcSpell = 0,
    /// <summary>
    /// ReqParam1 is the required base attack bonus, eg; Level 5 fighter will have 5 BAB, while a Level 5 Wizard has 2, while a Fighter 5 / Wizard 5 has 7
    /// </summary>
    Bab,
    /// <summary>
    /// ReqParam1 is the row number of a required class in classes.2da. Meeting any of the CLASSOR prerequisites counts as meeting them all.
    /// </summary>
    ClassOr,
    /// <summary>
    /// ReqParam1 is the row number of a prohibited class in classes.2da.
    /// </summary>
    ClassNot,
    /// <summary>
    /// ReqParam1 is the row number of a required feat in feat.2da.
    /// </summary>
    Feat,
    /// <summary>
    /// ReqParam1 is the row number of a required feat in feat.2da. Meeting any of the FEATOR prerequisites counts as meeting them all.
    /// </summary>
    FeatOr,
    /// <summary>
    /// 	ReqParam1 is the row number of a permitted racial type in racialtypes.2da. Meeting any of the RACE prerequisites counts as meeting them all.
    /// </summary>
    Race,
    /// <summary>
    /// ReqParam1 indicates the type of save (1 for fortitude, 2 for reflex, or 3 for will), and ReqParam2 is the required saving throw bonus (without magical modifiers).
    /// </summary>
    Save,
    /// <summary>
    /// ReqParam1 is the row number of a required skill in skills.2da, and ReqParam2 is the required number of ranks in that skill.
    /// </summary>
    Skill,
    /// <summary>
    /// ReqParam1 is the required class level of a spellcasting class.
    /// </summary>
    Spell,
    /// <summary>
    /// ReqParam1 is the name of a local variable that will be checked that is set on the player object at the time, and ReqParam2 is the integer value this variable must have.
    /// </summary>
    Var
}
