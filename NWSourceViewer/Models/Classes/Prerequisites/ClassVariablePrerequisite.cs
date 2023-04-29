namespace NWSourceViewer.Models.Classes.Prerequisites;

/// <summary>
/// Indicates a local variable and its value that must be set in order to allow a class.
/// </summary>
public class ClassVariablePrerequisite
{
    public ClassVariablePrerequisite(string variableName, int value)
    {
        VariableName = variableName;
        Value = value;
    }

    /// <summary>
    /// The name of the variable.
    /// </summary>
    public string VariableName { get; }

    /// <summary>
    /// The value the variable must have.
    /// </summary>
    public int Value { get; }
}
