namespace NWSourceViewer.Models;

public class Config
{
    public uint MaxLevel { get; set; } = 40;
    public string TlkFileName { get; set; } = "custom.tlk";
    public bool IncludeEmpty2daRows { get; set; } = false;
    public bool IncludeUnuseableForPlayers2daRows { get; set;} = false;
}
