namespace NWSourceViewer.Models;

public class Config
{
    public uint MaxLevel { get; set; } = 40;
    public uint MaxPreEpicLevel { get; set; } = 20;
    public string TlkFileName { get; set; } = "";
    public string ClassesFileName { get; set; } = "classes";
    public string FeatsFileName { get; set; } = "feat";
    public string RacesFileName { get; set; } = "racialtypes";
    public string SkillsFileName { get; set; } = "skills";
    public string SpellsFileName { get; set; } = "spells";
}
