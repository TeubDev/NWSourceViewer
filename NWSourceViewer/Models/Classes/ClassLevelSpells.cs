namespace NWSourceViewer.Models.Classes
{
    /// <summary>
    /// Represents the number of spells known or gained at each spell level for a particular class level.
    /// </summary>
    public class ClassLevelSpells : Base2daRowModel
    {
        public override void ConvertData(Dictionary<string, string> data, TlkDictionary tlk)
        {
            Index = data["Index"].ToUint();
            if (data["Level"] != Constants.NullString)
            {
                HasData = true;
                SpellLevel0 = data.GetNUint("SpellLevel0");
                SpellLevel1 = data.GetNUint("SpellLevel1");
                SpellLevel2 = data.GetNUint("SpellLevel2");
                SpellLevel3 = data.GetNUint("SpellLevel3");
                SpellLevel4 = data.GetNUint("SpellLevel4");
                SpellLevel5 = data.GetNUint("SpellLevel5");
                SpellLevel6 = data.GetNUint("SpellLevel6");
                SpellLevel7 = data.GetNUint("SpellLevel7");
                SpellLevel8 = data.GetNUint("SpellLevel8");
                SpellLevel9 = data.GetNUint("SpellLevel9");
            }
        }

        public uint? SpellLevel0 { get; set; }
        public uint? SpellLevel1 { get; set; }
        public uint? SpellLevel2 { get; set; }
        public uint? SpellLevel3 { get; set; }
        public uint? SpellLevel4 { get; set; }
        public uint? SpellLevel5 { get; set; }
        public uint? SpellLevel6 { get; set; }
        public uint? SpellLevel7 { get; set; }
        public uint? SpellLevel8 { get; set; }
        public uint? SpellLevel9 { get; set; }
    }
}
