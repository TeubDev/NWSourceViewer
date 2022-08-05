namespace NWSourceViewer.Models;

public class TlkDictionary : Dictionary<uint, string>
{
    public new string this[uint key]
    {
        get
        {
            if (TryGetValue(key, out string? value) && value != null)
            {
                return value;
            }
            else if (TryGetValue(0, out string? defaultValue) && defaultValue != null)
            {
                return defaultValue;
            }
            return "Bad Strref";
        }
        set { base[key] = value; }
    }
}
