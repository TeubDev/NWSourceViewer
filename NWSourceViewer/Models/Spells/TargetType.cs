namespace NWSourceViewer.Models.Spells
{
    [Flags]
    public enum TargetType
    {
        Self = 1,
        Creature = 2,
        Area = 4,
        Item = 8,
        Door = 16,
        Placeable = 32,
        Trigger = 64,
    }
}
