﻿namespace NWSourceViewer.Models.Classes;

/// <summary>
/// Represents a row in the cls_bfeat_*.2da files.
/// </summary>
public class ClassBonusFeatModel : Base2daRowModel
{
    public override void ConvertData(Dictionary<string, string> data, TlkDictionary tlk)
    {
        Index = data["Index"].ToUint();
        var bonusDataValue = data["Bonus"];
        if (bonusDataValue != Constants.NullString)
        {
            HasData = true;
            Bonus = bonusDataValue.ToUint();
            UseableForPlayers = true;
        }
    }

    public uint Bonus { get; set; }
}
