namespace NWSourceViewer.Models;

public abstract class Base2daRowModel
{
    /// <summary>
    /// Called after the class is constructed to covert data to the properties represented in the file.
    /// </summary>
    /// <param name="data">Usually represents a table row, where the 2da column header is the key.</param>
    public abstract void ConvertData(Dictionary<string, string> data);

    public bool HasData { get; set; } = false;
    public uint Index { get; set; } = 0;
}
