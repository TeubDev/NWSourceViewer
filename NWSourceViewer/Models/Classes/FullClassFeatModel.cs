using NWSourceViewer.Models.Feats;

namespace NWSourceViewer.Models.Classes;

public class FullClassFeatModel
{
    public FullClassFeatModel(ClassFeatModel classFeat, FeatModel feat)
    {
        ClassFeat = classFeat;
        Feat = feat;
    }
    public ClassFeatModel ClassFeat { get; set; }

    public FeatModel Feat { get; set; }
}
