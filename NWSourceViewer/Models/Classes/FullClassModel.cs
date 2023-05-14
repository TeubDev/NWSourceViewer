using NWSourceViewer.Models.Classes.AlignmentRestrictions;
using NWSourceViewer.Models.Classes.Prerequisites;
using NWSourceViewer.Models.Feats;
using NWSourceViewer.Models.Spells;

namespace NWSourceViewer.Models.Classes;

public class FullClassModel
{
    public FullClassModel(ClassModel classModel)
    {
        ClassModel = classModel;
    }

    /// <summary>
    /// Base 2da row used for looking up most of the other information in this model.
    /// </summary>
    public ClassModel ClassModel { get; set; }

    /// <summary>
    /// List of information for each pre-epic level.
    /// </summary>
    public List<ClassLevelModel> ClassLevels { get; set; } = new List<ClassLevelModel>();

    /// <summary>
    /// List of information for each epic level.
    /// </summary>
    public List<ClassLevelModel> EpicClassLevels { get; set; } = new List<ClassLevelModel>();

    /// <summary>
    /// Feats available as general feats that might not be available to every class.
    /// </summary>
    public List<FeatModel> GeneralFeats { get; set; } = new List<FeatModel>();

    /// <summary>
    /// Feats selectable as general or bonus feats.
    /// </summary>
    public List<FeatModel> GeneralOrBonusFeats { get; set; } = new List<FeatModel>();

    /// <summary>
    /// Feats that can only be selected as bonus feats.
    /// </summary>
    public List<FeatModel> BonusFeats { get; set; } = new List<FeatModel>();

    /// <summary>
    /// Epic feats available as general feats that might not be available to every class.
    /// </summary>
    public List<FeatModel> EpicGeneralFeats { get; set; } = new List<FeatModel>();

    /// <summary>
    /// Epic feats selectable as general or bonus feats.
    /// </summary>
    public List<FeatModel> EpicGeneralOrBonusFeats { get; set; } = new List<FeatModel>();

    /// <summary>
    /// Epic feats that can only be selected as bonus feats.
    /// </summary>
    public List<FeatModel> EpicBonusFeats { get; set; } = new List<FeatModel>();

    /// <summary>
    /// Set to null if there are no prerequisites (other than alignment).
    /// </summary>
    public FullClassPrerequisiteModel? Prerequisites { get; set; } = null;
    public List<SkillModel> ClassSkills { get; set; } = new List<SkillModel>();
    public List<SkillModel> CrossClassSkills { get; set; } = new List<SkillModel>();
    public List<SkillModel> UnavailableSkills { get; set; } = new List<SkillModel>();
    public AlignmentsAllowed AlignmentsAllowed { get; set; } = new AlignmentsAllowed();

    /// <summary>
    /// Key is the spell level. Value is the list of spells at that level.
    /// </summary>
    public Dictionary<uint, List<SpellModel>> SpellLists { get; set; } = new Dictionary<uint, List<SpellModel>>();
}
