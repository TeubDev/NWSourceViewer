namespace NWSourceViewer.Models.Classes.Prerequisites;

public class ClassPrerequisiteModel : Base2daRowModel
{
    public override void ConvertData(Dictionary<string, string> data, TlkDictionary tlk)
    {
        Index = data["Index"].ToUint();
        var reqTypeDataValue = data["ReqType"];
        if (reqTypeDataValue != Constants.NullString && Enum.TryParse(reqTypeDataValue, true, out ClassPrerequisiteType reqType))
        {
            HasData = true;
            ReqType = reqType;
            if (ReqType == ClassPrerequisiteType.Var)
            {
                ScriptVar = data["ReqParam1"];
            }
            else
            {
                NumericReqParam1 = data["ReqParam1"].ToUint();
            }
            NumericReqParam2 = data["ReqParam2"].ToUint();
            UseableForPlayers = true;
        }
    }

    /// <summary>
    /// A set requirement type.
    /// </summary>
    public ClassPrerequisiteType ReqType { get; set; }
    /// <summary>
    /// See <see cref="ClassPrerequisiteType"/> for more information on what each of these mean for a given type.
    /// </summary>
    public uint NumericReqParam1 { get; set; }
    /// <summary>
    /// See <see cref="ClassPrerequisiteType"/> for more information on what each of these mean for a given type.
    /// </summary>
    public uint NumericReqParam2 { get; set; }
    /// <summary>
    /// Converted from ReqParam1 when <see cref="ReqType"/> is <see cref="ClassPrerequisiteType.Var"/>
    /// </summary>
    public string ScriptVar { get; set; } = "";
}
