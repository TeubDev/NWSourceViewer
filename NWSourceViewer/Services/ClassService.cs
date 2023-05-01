using NWSourceViewer.Components.Classes;
using NWSourceViewer.Models;
using NWSourceViewer.Models.Classes;
using NWSourceViewer.Models.Classes.Prerequisites;
using NWSourceViewer.Models.Feats;
using NWSourceViewer.Models.Races;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NWSourceViewer.Services;

/// <summary>
/// For interacting with classes.
/// </summary>
public interface IClassService
{
    /// <summary>
    /// Gets the class for the given ID along with all other subtables. Returns null if the class or one of its subclasses was not found.
    /// </summary>
    Task<FullClassModel?> GetFullClassModelAsync(uint classId, CancellationToken cancellationToken);
}

/// <inheritdoc cref="IClassService" />
public class ClassService : IClassService
{
    private readonly IFileLoader fileLoader;
    private readonly Config config;

    public ClassService(IFileLoader fileLoader, Config config)
    {
        this.fileLoader = fileLoader;
        this.config = config;
    }

    public async Task<FullClassModel?> GetFullClassModelAsync(uint classId, CancellationToken cancellationToken)
    {
        FullClassModel? fullClassModel = null;
        var classModelTask = fileLoader.Load2daRowAsync<ClassModel>(Constants.ClassesFileName, classId, cancellationToken);
        var featsTableTask = fileLoader.Load2daAsync<FeatModel>(Constants.FeatsFileName, cancellationToken);
        await Task.WhenAll(classModelTask, featsTableTask);
        var classModel = await classModelTask;
        var featsTable = await featsTableTask;
        if (classModel != null && featsTable != null)
        {
            fullClassModel = new FullClassModel(classModel);
            await Task.WhenAll(
                SetClassPrerequisitesAsync(fullClassModel, featsTable, cancellationToken),
                SetClassLevelsAsync(fullClassModel, featsTable, cancellationToken),
                SetSkillsAsync(fullClassModel, cancellationToken)
                );
        }

        return fullClassModel;
    }

    private async Task SetSkillsAsync(FullClassModel fullClass, CancellationToken cancellationToken)
    {
        if (fullClass.ClassModel.SkillsTable != Constants.NullString)
        {
            var skillsTask = fileLoader.Load2daAsync<SkillModel>(Constants.SkillsFileName, cancellationToken);
            var classSkillsTask = fileLoader.Load2daAsync<ClassSkillModel>(fullClass.ClassModel.SkillsTable, cancellationToken);
            await Task.WhenAll(skillsTask, classSkillsTask);
            var skills = await skillsTask;
            var classSkills = await classSkillsTask;
            if (skills != null && classSkills != null)
            {
                foreach (var skill in skills)
                {
                    var matchingClassSkill = classSkills.FirstOrDefault(classSkill => classSkill.HasData && classSkill.SkillIndex == skill.Index);
                    if (matchingClassSkill != null)
                    {
                        if (matchingClassSkill.ClassSkill)
                        {
                            fullClass.ClassSkills.Add(skill);
                        }
                        else
                        {

                            fullClass.CrossClassSkills.Add(skill);
                        }
                    }
                    else
                    {
                        fullClass.UnavailableSkills.Add(skill);
                    }
                }
            }
        }
    }

    private async Task SetClassLevelsAsync(FullClassModel fullClass, List<FeatModel> featsTable, CancellationToken cancellationToken)
    {
        var abTableTask = fileLoader.Load2daAsync<ClassAttackBonusModel>(fullClass.ClassModel.AttackBonusTable, cancellationToken);
        var savesTableTask = fileLoader.Load2daAsync<ClassSavingThrowModel>(fullClass.ClassModel.SavingThrowTable, cancellationToken);
        var classFeatTableTask = fileLoader.Load2daAsync<ClassFeatModel>(fullClass.ClassModel.FeatsTable, cancellationToken);
        var bonusFeatTableTask = fileLoader.Load2daAsync<ClassBonusFeatModel>(fullClass.ClassModel.BonusFeatsTable, cancellationToken);
        var spellsKnownTableTask = fileLoader.Load2daAsync<ClassLevelSpellsKnown>(fullClass.ClassModel.SpellKnownTable, cancellationToken);
        await Task.WhenAll(abTableTask, savesTableTask, classFeatTableTask, bonusFeatTableTask, spellsKnownTableTask);
        var abTable = await abTableTask;
        var savesTable = await savesTableTask;
        var classFeatTable = await classFeatTableTask;
        var bonusFeatTable = await bonusFeatTableTask;
        var spellsKnownTable = await spellsKnownTableTask;
        if (abTable != null && savesTable != null && classFeatTable != null && bonusFeatTable != null)
        {
            var (maxPreEpicLevel, maxLevel) = fullClass.ClassModel.GetMaxLevels(Constants.MaxPreEpicLevel, config.MaxLevel);
            uint hpMin = 0;
            uint hpMax = 0;
            for (int i = 0; i < maxLevel; i++)
            {
                uint level = (uint)i + 1;
                hpMin += fullClass.ClassModel.HitDie / 2;
                hpMax += fullClass.ClassModel.HitDie;
                var classFeats = classFeatTable.Where(classFeat => classFeat.HasData && classFeat.GrantedOnLevel == level);

                var classLevel = new ClassLevelModel
                {
                    Level = level,
                    BaseAttackBonus = abTable[i].Bab, // TODO: Fix BAB and saves for epic levels.
                    HitPointMinimum = hpMin,
                    HitPointMaximum = hpMax,
                    FortitudeSave = savesTable[i].FortSave,
                    ReflexSave = savesTable[i].RefSave,
                    WillSave = savesTable[i].WillSave,
                    BonusFeatCount = bonusFeatTable[i].Bonus,
                    AutomaticFeats = classFeats
                                    .Where(cf => cf.List == ClassFeatType.AutomaticallyGrantedFeat)
                                    .Select(cf => featsTable[(int)cf.FeatIndex])
                                    .ToList(),
                    SpellsKnown = spellsKnownTable?.FirstOrDefault(s => s.Index == i)
                };
                if (classLevel.Level <= maxPreEpicLevel)
                {
                    fullClass.ClassLevels.Add(classLevel);
                }
                else
                {
                    fullClass.EpicClassLevels.Add(classLevel);
                }
            }
        }
    }

    private async Task SetClassPrerequisitesAsync(FullClassModel fullClass, List<FeatModel> featsTable, CancellationToken cancellationToken)
    {
        if (fullClass.ClassModel.PreReqTable != Constants.NullString)
        {
            var prerequisites = await fileLoader.Load2daAsync<ClassPrerequisiteModel>(fullClass.ClassModel.PreReqTable, cancellationToken);
            if (prerequisites != null)
            {
                fullClass.Prerequisites = new FullClassPrerequisiteModel();
                foreach (var prerequisite in prerequisites)
                {
                    switch (prerequisite.ReqType)
                    {
                        case ClassPrerequisiteType.ArcSpell:
                            fullClass.Prerequisites.ArcaneSpellcastingLevel = prerequisite.NumericReqParam1;
                            break;
                        case ClassPrerequisiteType.Bab:
                            fullClass.Prerequisites.Bab = prerequisite.NumericReqParam1;
                            break;
                        case ClassPrerequisiteType.ClassOr:
                            var matchingClass = await fileLoader.Load2daRowAsync<ClassModel>(Constants.ClassesFileName, prerequisite.NumericReqParam1, cancellationToken);
                            if (matchingClass != null)
                            {
                                fullClass.Prerequisites.OrClasses.Add(matchingClass);
                            }
                            break;
                        case ClassPrerequisiteType.Feat:
                            var matchingFeat = featsTable.FirstOrDefault(f => f.Index == prerequisite.NumericReqParam1);
                            if (matchingFeat != null)
                            {
                                fullClass.Prerequisites.Feats.Add(matchingFeat);
                            }
                            break;
                        case ClassPrerequisiteType.FeatOr:
                            var matchingOrFeat = featsTable.FirstOrDefault(f => f.Index == prerequisite.NumericReqParam1);
                            if (matchingOrFeat != null)
                            {
                                fullClass.Prerequisites.OrFeats.Add(matchingOrFeat);
                            }
                            break;
                        case ClassPrerequisiteType.Race:
                            var matchingRace = await fileLoader.Load2daRowAsync<RaceModel>(Constants.RacesFileName, prerequisite.NumericReqParam1, cancellationToken);
                            if (matchingRace != null)
                            {
                                fullClass.Prerequisites.Races.Add(matchingRace);
                            }
                            break;
                        case ClassPrerequisiteType.Skill:
                            var matchingSkill = await fileLoader.Load2daRowAsync<SkillModel>(Constants.SkillsFileName, prerequisite.NumericReqParam1, cancellationToken);
                            if (matchingSkill != null)
                            {
                                fullClass.Prerequisites.Skills.Add(new ClassSkillPrerequisite(matchingSkill, prerequisite.NumericReqParam2));
                            }
                            break;
                        case ClassPrerequisiteType.Spell:
                            fullClass.Prerequisites.MinSpellcastingLevel = prerequisite.NumericReqParam1;
                            break;
                        case ClassPrerequisiteType.Var:
                            fullClass.Prerequisites.Variable = new ClassVariablePrerequisite(
                                prerequisite.ScriptVar ?? "",
                                (int)prerequisite.NumericReqParam2);
                            break;
                    }
                }
            }
        }
    }
}
