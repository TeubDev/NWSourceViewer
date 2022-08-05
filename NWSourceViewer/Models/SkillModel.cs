namespace NWSourceViewer.Models;

public class SkillModel : Base2daRowModel
{
    public override void ConvertData(Dictionary<string, string> data)
    {
        Index = data["Index"].ToUint();
        var nameDataValue = data["Name"];
        if (nameDataValue != "****")
        {
            HasData = true;
            Name = nameDataValue.ToUint();
            Description = data["Description"].ToUint();
            Untrained = data["Untrained"].ToBool();
            KeyAbility = data["KeyAbility"];
            ArmorCheckPenalty = data["ArmorCheckPenalty"].ToBool();
        }
    }

    public uint Name { get; set; } = 0;
    public uint Description { get; set; } = 0;
    public bool Untrained { get; set; } = false;
    public string KeyAbility { get; set; } = "";
    public bool ArmorCheckPenalty { get; set; } = false;
}
