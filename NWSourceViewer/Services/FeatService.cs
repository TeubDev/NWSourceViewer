using NWSourceViewer.Models;
using NWSourceViewer.Models.Feats;

namespace NWSourceViewer.Services;

public class FeatService
{
    private readonly IFileLoader fileLoader;
    private readonly Config config;

    public FeatService(IFileLoader fileLoader, Config config)
    {
        this.fileLoader = fileLoader;
        this.config = config;
    }

    public async Task<FeatModel?> GetFeatModelAsync(uint featId, CancellationToken cancellationToken)
    {
        var feats = await fileLoader.Load2daAsync<FeatModel>("feat", cancellationToken);
        if (feats?.Count > featId)
        {
            return feats[(int)featId];
        }
        return null;
    }

    public async Task<FullFeatModel?> GetFullFeatModelAsync(uint featId, CancellationToken cancellationToken)
    {
        FullFeatModel? fullFeatModel = null;
        var featmodel = await GetFeatModelAsync(featId, cancellationToken);
        if (featmodel != null)
        {

        }
        return fullFeatModel;
    }
}
