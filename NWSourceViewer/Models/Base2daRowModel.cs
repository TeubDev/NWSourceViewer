namespace NWSourceViewer.Models;

public abstract class Base2daRowModel
{
    /// <summary>
    /// Called after the class is constructed to covert data to the properties represented in the file.
    /// </summary>
    /// <param name="data">Usually represents a table row, where the 2da column header is the key.</param>
    public abstract void ConvertData(Dictionary<string, string> data, TlkDictionary tlk);

    /// <summary>
    /// True if proper data was loaded into the row.
    /// </summary>
    public bool HasData { get; set; } = false;

    /// <summary>
    /// True if this row can be used by players.
    /// </summary>
    public bool UseableForPlayers { get; set; } = false;

    /// <summary>
    /// The row number in the 2da file.
    /// </summary>
    public uint Index { get; set; } = 0;

    /// <summary>
    /// The value of the name after being looked up from the TLK file.
    /// </summary>
    public string NameString { get; set; } = "";

    /// <summary>
    /// The value of the description after being looked up from the TLK file.
    /// </summary>
    public string DescriptionString { get; set; } = "";
}
