namespace NWSourceViewer.Models.Classes;

/// <summary>
/// Represents a row from one of the cls_atk_*.2da files.
/// </summary>
public class ClassAttackBonusModel : Base2daRowModel
{
    public override void ConvertData(Dictionary<string, string> data, TlkDictionary tlk)
    {
        Index = data["Index"].ToUint();
        string babDataValue = data["BAB"];
        if (babDataValue != Constants.NullString)
        {
            HasData = true;
            Bab = babDataValue.ToUint();
            UseableForPlayers = true;
        }
    }

    public uint Bab { get; set; }
}
