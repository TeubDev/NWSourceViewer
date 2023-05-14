using NWSourceViewer.Models;
using NWSourceViewer.Models.Classes;
using NWSourceViewer.Models.Classes.AlignmentRestrictions;
using NWSourceViewer.Models.Classes.Prerequisites;
using NWSourceViewer.Models.Feats;
using NWSourceViewer.Models.Races;
using NWSourceViewer.Models.Spells;

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
            var classFeatTable = await fileLoader.Load2daAsync<ClassFeatModel>(classModel.FeatsTable, cancellationToken);
            if (classFeatTable != null)
            {
                IEnumerable<FullClassFeatModel> classFeats = classFeatTable
                .Where(cf => cf.HasData)
                .Join(featsTable,
                    cf => cf.FeatIndex,
                    f => f.Index,
                    (cf, f) => new FullClassFeatModel(cf, f));

                fullClassModel = new FullClassModel(classModel);
                await Task.WhenAll(
                    SetClassPrerequisitesAsync(fullClassModel, featsTable, cancellationToken),
                    SetClassLevelsAsync(fullClassModel, classFeats, cancellationToken),
                    SetSkillsAsync(fullClassModel, cancellationToken),
                    SetSpellListAsync(fullClassModel, cancellationToken)
                    );
                SetAlignmentsAllowed(fullClassModel);
                SetGeneralAndBonusFeats(fullClassModel, classFeats);
            }
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

    private async Task SetClassLevelsAsync(FullClassModel fullClass, IEnumerable<FullClassFeatModel> classFeats, CancellationToken cancellationToken)
    {
        var abTableTask = fileLoader.Load2daAsync<ClassAttackBonusModel>(fullClass.ClassModel.AttackBonusTable, cancellationToken);
        var savesTableTask = fileLoader.Load2daAsync<ClassSavingThrowModel>(fullClass.ClassModel.SavingThrowTable, cancellationToken);
        var bonusFeatTableTask = fileLoader.Load2daAsync<ClassBonusFeatModel>(fullClass.ClassModel.BonusFeatsTable, cancellationToken);
        var spellsKnownTableTask = fileLoader.Load2daAsync<ClassLevelSpells>(fullClass.ClassModel.SpellKnownTable, cancellationToken);
        var spellsGainedTableTask = fileLoader.Load2daAsync<ClassLevelSpells>(fullClass.ClassModel.SpellGainTable, cancellationToken);
        await Task.WhenAll(abTableTask, savesTableTask, bonusFeatTableTask, spellsKnownTableTask);
        var abTable = await abTableTask;
        var savesTable = await savesTableTask;
        var bonusFeatTable = await bonusFeatTableTask;
        var spellsKnownTable = await spellsKnownTableTask;
        var spellsGainedTable = await spellsGainedTableTask;
        if (abTable != null && savesTable != null && bonusFeatTable != null)
        {
            var (maxPreEpicLevel, maxLevel) = fullClass.ClassModel.GetMaxLevels(Constants.MaxPreEpicLevel, config.MaxLevel);
            uint hpMin = 0;
            uint hpMax = 0;
            for (int i = 0; i < maxLevel; i++)
            {
                uint level = (uint)i + 1;
                hpMin += fullClass.ClassModel.HitDie / 2;
                hpMax += fullClass.ClassModel.HitDie;

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
                                    .Where(cf => cf.ClassFeat.List == ClassFeatType.AutomaticallyGrantedFeat
                                        && cf.ClassFeat.GrantedOnLevel == level)
                                    .Select(cf => cf.Feat)
                                    .OrderBy(f => f.NameString)
                                    .ToList(),
                    SpellsKnown = spellsKnownTable?.FirstOrDefault(s => s.Index == i),
                    SpellsGained = spellsGainedTable?.FirstOrDefault(s => s.Index == i),
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

    private async Task SetSpellListAsync(FullClassModel fullClass, CancellationToken cancellationToken)
    {
        
        if (fullClass.ClassModel.SpellTableColumn != Constants.NullString)
        {
            var spellsTable = await fileLoader.Load2daAsync<SpellModel>(Constants.SpellsFileName, cancellationToken);
            if (spellsTable != null)
            {
                var allClassSpells = spellsTable
                    .Where(spell => spell.HasData)
                    .GroupBy(spell => spell.GetLevelForClass(fullClass.ClassModel.SpellTableColumn))
                    .Where(spellLevelGroup => spellLevelGroup.Key != null)
                    .OrderBy(spellLevelGroup => spellLevelGroup.Key);
                    foreach (var spellLevelList in allClassSpells)
                    {
                        fullClass.SpellLists[spellLevelList.Key!.Value] = spellLevelList
                            .OrderBy(s => s.NameString)
                            .ToList();
                    }
            }
        }
    }

    private void SetGeneralAndBonusFeats(FullClassModel fullClass, IEnumerable<FullClassFeatModel> classFeats)
    {
        fullClass.GeneralFeats = classFeats
            .Where(cf => cf.ClassFeat.List == ClassFeatType.GeneralFeat
                && cf.Feat.PreReqEpic == false)
            .Select(cf => cf.Feat)
            .ToList();
        fullClass.GeneralOrBonusFeats = classFeats
            .Where(cf => cf.ClassFeat.List == ClassFeatType.GeneralOrBonusFeat
                && cf.Feat.PreReqEpic == false)
            .Select(cf => cf.Feat)
            .ToList();
        fullClass.BonusFeats = classFeats
            .Where(cf => cf.ClassFeat.List == ClassFeatType.BonusFeatOnly
                && cf.Feat.PreReqEpic == false)
            .Select(cf => cf.Feat)
            .ToList();

        fullClass.EpicGeneralFeats = classFeats
            .Where(cf => cf.ClassFeat.List == ClassFeatType.GeneralFeat
                && cf.Feat.PreReqEpic == true)
            .Select(cf => cf.Feat)
            .ToList();
        fullClass.EpicGeneralOrBonusFeats = classFeats
            .Where(cf => cf.ClassFeat.List == ClassFeatType.GeneralOrBonusFeat
                && cf.Feat.PreReqEpic == true)
            .Select(cf => cf.Feat)
            .ToList();
        fullClass.EpicBonusFeats = classFeats
            .Where(cf => cf.ClassFeat.List == ClassFeatType.BonusFeatOnly
                && cf.Feat.PreReqEpic == true)
            .Select(cf => cf.Feat)
            .ToList();
    }

    private void SetAlignmentsAllowed(FullClassModel fullClass)
    {
        fullClass.AlignmentsAllowed = Helpers.GetAlignmentsAllowed(
            fullClass.ClassModel.AlignRestrict,
            fullClass.ClassModel.AlignRstrctType,
            fullClass.ClassModel.InvertRestrict);
    }
}
