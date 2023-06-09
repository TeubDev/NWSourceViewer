﻿namespace NWSourceViewer.Models;

public class SkillModel : Base2daRowModel
{
    public override void ConvertData(Dictionary<string, string> data, TlkDictionary tlk)
    {
        Index = data["Index"].ToUint();
        var nameDataValue = data["Name"];
        if (nameDataValue != Constants.NullString)
        {
            HasData = true;
            Name = nameDataValue.ToUint();
            NameString = tlk[Name];
            Description = data["Description"].ToUint();
            DescriptionString = tlk[Description];
            Untrained = data["Untrained"].ToBool();
            KeyAbility = data["KeyAbility"];
            ArmorCheckPenalty = data["ArmorCheckPenalty"].ToBool();
            UseableForPlayers = true;
        }
    }

    public uint Name { get; set; } = 0;
    public uint Description { get; set; } = 0;
    public bool Untrained { get; set; } = false;
    public string KeyAbility { get; set; } = "";
    public bool ArmorCheckPenalty { get; set; } = false;
}
