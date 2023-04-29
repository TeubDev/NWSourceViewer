namespace NWSourceViewer.Models.Classes.Prerequisites;

/// <summary>
/// Represents a skill and a number of ranks required for it.
/// </summary>
public class ClassSkillPrerequisite
{
    public ClassSkillPrerequisite(SkillModel skill, uint ranks)
    {
        Skill = skill;
        Ranks = ranks;
    }

    /// <summary>
    /// The skill being specified
    /// </summary>
    public SkillModel Skill { get; set; }

    /// <summary>
    /// The number of ranks of <see cref="Skill"/> required.
    /// </summary>
    public uint Ranks { get; set; }
}
