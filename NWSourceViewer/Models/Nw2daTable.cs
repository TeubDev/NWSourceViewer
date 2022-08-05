namespace NWSourceViewer.Models;

public class Nw2daTable // TODO: Check references and delete if unused.
{
    public Nw2daTable(string rawData)
    {
        string[]? rows = rawData.Split("\r\n");
        char[]? splitCharacters = null;
        var headers = rows[2].Split(splitCharacters, StringSplitOptions.RemoveEmptyEntries).ToList();
        headers.Insert(0, "Index");
        Headers = headers;
        Data = new();
        for (int rowIndex = 3; rowIndex < rows.Length; rowIndex++)
        {
            if (!string.IsNullOrWhiteSpace(rows[rowIndex]))
            {
                var row = rows[rowIndex].Split(splitCharacters, StringSplitOptions.RemoveEmptyEntries);
                Dictionary<string, string> dataRow = new();
                for (int colIndex = 0; colIndex < row.Length; colIndex++)
                {
                    dataRow[Headers[colIndex]] = row[colIndex];
                }
                Data.Add(dataRow);
            }
        }
    }

    public List<string> Headers { get; }

    public List<Dictionary<string, string>> Data { get; }
}
