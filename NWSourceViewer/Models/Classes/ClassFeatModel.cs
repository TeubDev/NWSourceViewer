namespace NWSourceViewer.Models.Classes;

/// <summary>
/// Indicates how a class can or cannot get a particular feat.
/// </summary>
public class ClassFeatModel : Base2daRowModel
{
    public override void ConvertData(Dictionary<string, string> data, TlkDictionary tlk)
    {
        Index = data["Index"].ToUint();
        var featIndexDataValue = data["FeatIndex"];
        if (featIndexDataValue != Constants.NullString)
        {
            HasData = true;
            FeatIndex = featIndexDataValue.ToUint();
            List = (ClassFeatType)data["List"].ToUint();
            GrantedOnLevel = data["GrantedOnLevel"].ToInt();
            OnMenu = data["OnMenu"].ToUint();
            UseableForPlayers = true;
        }
    }

    /// <summary>
    /// Index of the feat in feats.2da
    /// </summary>
    public uint FeatIndex { get; set; }
    /// <summary>
    /// This is to define how the feat is available or granted.
    /// </summary>
    public ClassFeatType List { get; set; }
    /// <summary>
    /// Level when the feat is granted. -1 means any levelup.
    /// </summary>
    public int GrantedOnLevel { get; set; }
    /// <summary>
    /// How it shows up on menus.
    /// </summary>
    public uint OnMenu { get; set; }
}
