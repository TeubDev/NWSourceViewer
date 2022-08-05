using NWSourceViewer.Models;
using NWSourceViewer.Models.Classes;
using NWSourceViewer.Models.Feats;

namespace NWSourceViewer.Services;

/// <summary>
/// For interacting with classes.
/// </summary>
public interface IClassService
{
    /// <summary>
    /// Gets the class for the given ID. Returns null if the class was not found.
    /// </summary>
    Task<ClassModel?> GetClassModelAsync(uint classId, CancellationToken cancellationToken);

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

    public async Task<ClassModel?> GetClassModelAsync(uint classId, CancellationToken cancellationToken)
    {
        var classes = await fileLoader.Load2daAsync<ClassModel>("classes", cancellationToken);
        if (classes?.Count > classId)
        {
            return classes[(int)classId];
        }
        return null;
    }

    public async Task<FullClassModel?> GetFullClassModelAsync(uint classId, CancellationToken cancellationToken)
    {
        FullClassModel? fullClassModel = null;
        var classModel = await GetClassModelAsync(classId, cancellationToken);
        if (classModel != null)
        {
            var (maxPreEpicLevel, maxLevel) = classModel.GetMaxLevels(config.MaxPreEpicLevel, config.MaxLevel);
            var tlkTask = fileLoader.LoadTlkAsync(cancellationToken);
            var abTableTask = fileLoader.Load2daAsync<ClassAttackBonusModel>(classModel.AttackBonusTable, cancellationToken);
            var savesTableTask = fileLoader.Load2daAsync<ClassSavingThrowModel>(classModel.SavingThrowTable, cancellationToken);
            var classFeatTableTask = fileLoader.Load2daAsync<ClassFeatModel>(classModel.FeatsTable, cancellationToken);
            var bonusFeatTableTask = fileLoader.Load2daAsync<ClassBonusFeatModel>(classModel.BonusFeatsTable, cancellationToken);
            var featTableTask = fileLoader.Load2daAsync<FeatModel>("feat", cancellationToken);
            // TODO: load other tables.
            await Task.WhenAll(tlkTask, abTableTask, savesTableTask, classFeatTableTask, bonusFeatTableTask, featTableTask);
            var tlk = await tlkTask;
            var abTable = await abTableTask;
            var savesTable = await savesTableTask;
            var classFeatTable = await classFeatTableTask;
            var bonusFeatTable = await bonusFeatTableTask;
            var featsTable = await featTableTask;
            if (abTable != null && savesTable != null && classFeatTable != null && bonusFeatTable != null && featsTable != null)
            {
                fullClassModel = new FullClassModel(classModel);
                uint hpMin = 0;
                uint hpMax = 0;
                for (int i = 0; i < maxLevel; i++)
                {
                    uint level = (uint)i + 1;
                    hpMin += classModel.HitDie / 2;
                    hpMax += classModel.HitDie;
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
                                        .ToList()
                    };
                    if (classLevel.Level <= maxPreEpicLevel)
                    {
                        fullClassModel.ClassLevels.Add(classLevel);
                    }
                    else
                    {
                        fullClassModel.EpicClassLevels.Add(classLevel);
                    }
                }
            }
        }

        return fullClassModel;
    }
}
