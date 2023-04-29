namespace NWSourceViewer.Models.Classes;

/// <summary>
/// Represents one row in the cls_savthr_*.2da files.
/// </summary>
public class ClassSavingThrowModel : Base2daRowModel
{
    public override void ConvertData(Dictionary<string, string> data, TlkDictionary tlk)
    {
        Index = data["Index"].ToUint();
        string fortDataValue = data["FortSave"];
        if (fortDataValue != Constants.NullString)
        {
            HasData = true;
            FortSave = fortDataValue.ToUint();
            RefSave = data["RefSave"].ToUint();
            WillSave = data["WillSave"].ToUint();
        }
    }
    public uint FortSave { get; set; }
    public uint RefSave { get; set; }
    public uint WillSave { get; set; }
}
