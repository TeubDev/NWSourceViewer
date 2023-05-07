namespace NWSourceViewer.Models.Classes;
public class ClassSkillModel : Base2daRowModel
{
    public override void ConvertData(Dictionary<string, string> data, TlkDictionary tlk)
    {
        Index = data["Index"].ToUint();
        SkillLabel = data["SkillLabel"];
        if (SkillLabel != Constants.NullString)
        {
            HasData = true;
            SkillIndex = data["SkillIndex"].ToUint();
            ClassSkill = data["ClassSkill"].ToBool();
            UseableForPlayers = true;
        }
    }

    public string SkillLabel { get; set; } = "";
    public uint SkillIndex { get; set; } = 0;
    public bool ClassSkill { get; set; } = false;
}
