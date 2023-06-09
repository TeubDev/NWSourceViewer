﻿using NWSourceViewer.Models.Feats;
using NWSourceViewer.Models.Races;

namespace NWSourceViewer.Models.Classes.Prerequisites;

public class FullClassPrerequisiteModel
{
    /// <summary>
    /// The required combined class levels which are arcane spellcasters  - ie is a spellcaster and has Arcane set to 1 in classes.2da.
    /// </summary>
    /// <remarks>
    /// null indicates no arcane spellcasting is required.
    /// </remarks>
    public uint ArcaneSpellcastingLevel { get; set; }

    /// <summary>
    /// The required base attack bonus
    /// </summary>
    /// <remarks>
    /// null indicates no base attack bonus is required.
    /// </remarks>
    public uint Bab { get; set; }

    /// <summary>
    /// A list of classes where at least one of them matching is enough to fulfill the prerequisite.
    /// </summary>
    public List<ClassModel> OrClasses { get; set; } = new List<ClassModel>();

    /// <summary>
    /// A list of all the feats the character must have.
    /// </summary>
    public List<FeatModel> Feats { get; set; } = new List<FeatModel>();

    /// <summary>
    /// A list of feats where at least one of them matching is enough to fulfill the prerequisite.
    /// </summary>
    public List<FeatModel> OrFeats { get; set; } = new List<FeatModel>();

    /// <summary>
    /// A list of races where at least one of them matching is enough to fulfill the prerequisite.
    /// </summary>
    public List<RaceModel> Races { get; set; } = new List<RaceModel>();

    /// <summary>
    /// A list of skills and number of ranks required for each. All must be fulfilled in order to qualify for the class.
    /// </summary>
    public List<ClassSkillPrerequisite> Skills { get; set; } = new List<ClassSkillPrerequisite>();

    /// <summary>
    /// The required level a character needs to have in at least one spellcasting class to qualify for the class.
    /// </summary>
    /// <remarks>
    /// null indicates no spell level requirements.
    /// </remarks>
    public uint MinSpellcastingLevel { get; set; }

    /// <summary>
    /// A variable that is required in order to take levels in a class.
    /// </summary>
    /// <remarks>
    /// null indicates no variable requirements.
    /// </remarks>
    public ClassVariablePrerequisite? Variable { get; set; }
}
