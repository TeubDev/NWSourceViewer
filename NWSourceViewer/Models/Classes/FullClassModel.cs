﻿using NWSourceViewer.Models.Classes.Prerequisites;
using NWSourceViewer.Models.Feats;

namespace NWSourceViewer.Models.Classes;

public class FullClassModel
{
    public FullClassModel(ClassModel classModel)
    {
        ClassModel = classModel;
    }
    public ClassModel ClassModel { get; set; }
    public List<ClassLevelModel> ClassLevels { get; set; } = new List<ClassLevelModel>();
    public List<ClassLevelModel> EpicClassLevels { get; set; } = new List<ClassLevelModel>();
    public List<FeatModel> GeneralOrBonusFeats { get; set; } = new List<FeatModel>();
    public List<FeatModel> BonusFeats { get; set; } = new List<FeatModel>();
    public List<FeatModel> EpicGeneralOrBonusFeats { get; set; } = new List<FeatModel>();
    public List<FeatModel> EpicBonusFeats { get; set; } = new List<FeatModel>();
    public FullClassPrerequisiteModel? Prerequisites { get; set; } = null;
    public List<SkillModel> ClassSkills { get; set; } = new List<SkillModel>();
    public List<SkillModel> CrossClassSkills { get; set; } = new List<SkillModel>();
    public List<SkillModel> UnavailableSkills { get; set; } = new List<SkillModel>();
}
