namespace NWSourceViewer.Models.Races;

public class RaceModel : Base2daRowModel
{
    public override void ConvertData(Dictionary<string, string> data, TlkDictionary tlk)
    {
        Index = data["Index"].ToUint();
        string nameDataValue = data["Name"];
        if (nameDataValue != Constants.NullString)
        {
            HasData = true;
            Name = nameDataValue.ToUint();
            NameString = tlk[Name];
            Description = data["Description"].ToUint();
            StrengthAdjustment = data["StrAdjust"].ToInt();
            DexterityAdjustment = data["DexAdjust"].ToInt();
            IntelligenceAdjustment = data["IntAdjust"].ToInt();
            CharismaAdjustment = data["ChaAdjust"].ToInt();
            WisdomAdjustment = data["WisAdjust"].ToInt();
            ConstitutionAdjustment = data["ConAdjust"].ToInt();
            FavoredClass = data["Favored"].ToNUint();
            FeatsTable = data["FeatsTable"];
            PlayerRace = data["PlayerRace"].ToBool();
            ExtraFeatsAtFirstLevel = data["ExtraFeatsAtFirstLevel"].ToUint();
            ExtraSkillPointsPerLevel = data["ExtraSkillPointsPerLevel"].ToUint();
        }
    }

    /// <summary>
    /// A StringRef for the name of this race (capitalized).
    /// </summary>
    public uint Name { get; set; } = 0;

    /// <summary>
    /// A StringRef for the description of this race (shown during character creation).
    /// </summary>
    public uint Description { get; set; }

    /// <summary>
    /// The racial modifier to strength for members of this race.
    /// </summary>
    public int StrengthAdjustment { get; set; }

    /// <summary>
    /// The racial modifier to dexterity for members of this race.
    /// </summary>
    public int DexterityAdjustment { get; set; }

    /// <summary>
    /// The racial modifier to intelligence for members of this race.
    /// </summary>
    public int IntelligenceAdjustment { get; set; }

    /// <summary>
    /// The racial modifier to charisma for members of this race.
    /// </summary>
    public int CharismaAdjustment { get; set; }

    /// <summary>
    /// The racial modifier to wisdom for members of this race.
    /// </summary>
    public int WisdomAdjustment { get; set; }

    /// <summary>
    /// The racial modifier to constitution for members of this race.
    /// </summary>
    public int ConstitutionAdjustment { get; set; }

    /// <summary>
    /// The racial modifier to constitution for members of this race.
    /// </summary>
    public uint? FavoredClass { get; set; }

    /// <summary>
    /// The name of the .2da file (without the extension) defining the racial feats gained from taking this race.
    /// </summary>
    public string FeatsTable { get; set; } = "";

    /// <summary>
    /// True if this race can be chosen by players. False if not.
    /// </summary>
    public bool PlayerRace { get; set; }

    /// <summary>
    /// If set it adds this many extra feats at first level - ie; human trait sets this to 1
    /// </summary>
    public uint ExtraFeatsAtFirstLevel { get; set; }

    /// <summary>
    /// If set it adds this many extra skill points at each level up. ie; humans have this set to 1.
    /// </summary>
    public uint ExtraSkillPointsPerLevel { get; set; }
}
