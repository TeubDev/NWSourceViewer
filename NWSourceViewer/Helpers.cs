﻿using NWSourceViewer.Models;
using NWSourceViewer.Models.Classes;
using NWSourceViewer.Models.Classes.AlignmentRestrictions;
using System.ComponentModel;

namespace NWSourceViewer;

public static class Helpers
{
    /// <summary>
    /// Creates a subarray of the array for the given length starting at the offest.
    /// </summary>
    /// <param name="array">The array from which to get the subarray.</param>
    /// <param name="offset">The index of the array to start the subarray.</param>
    /// <param name="length">The number of items to fetch out of the array.</param>
    private static T[] SubArray<T>(this T[] array, uint offset, uint length = 4)
    {
        T[] result = new T[length];
        Array.Copy(array, offset, result, 0, length);
        return result;
    }

    /// <summary>
    /// Converts the bytes in the array at a given location to a string. This assumes each byte is a character.
    /// </summary>
    private static string SubString(this byte[] array, uint offset, uint length = 4)
    {
        var subArray = array.SubArray(offset, length);
        return System.Text.Encoding.UTF8.GetString(subArray);
    }

    /// <summary>
    /// Converts the bytes in the array at a given location to a 32-bit unsigned integer.
    /// </summary>
    private static uint SubUInt(this byte[] array, uint offset, uint length = 4)
    {
        var subArray = array.SubArray(offset, length);
        return BitConverter.ToUInt32(subArray, 0);
    }

    /// <summary>
    /// Adds the values from the tlk file into the dictionary.
    /// </summary>
    /// <param name="dictionary">The dictionary to which the items should be added.</param>
    /// <param name="fileContents">The file contents to be parsed for strings.</param>
    /// <param name="offset">Added to the index for each entry.</param>
    public static void AddTlkFile(this TlkDictionary dictionary, byte[] fileContents, uint offset = 0)
    {
        var stringEntriesOffset = fileContents.SubUInt(16);
        for (uint stringDataStart = 20; stringDataStart < stringEntriesOffset; stringDataStart += 40)
        {
            string stringEntry;
            var stringSize = fileContents.SubUInt(stringDataStart + 32);
            if (stringSize > 0)
            {
                var offsetToString = fileContents.SubUInt(stringDataStart + 28);
                stringEntry = fileContents.SubString(stringEntriesOffset + offsetToString, stringSize);
            }
            else
            {
                stringEntry = "";
            }
            dictionary[offset] = stringEntry;
            offset++;
        }
    }

    /// <summary>
    /// Converts the string to a uint. If it cannot be converted, returns 0.
    /// </summary>
    public static uint ToUint(this string str)
    {
        return str.ToNUint() ?? 0;
    }

    /// <summary>
    /// Converts the string to a uint. If it cannot be converted, returns null.
    /// </summary>
    public static uint? ToNUint(this string str)
    {
        if (uint.TryParse(str, out uint value))
        {
            return value;
        }
        return null;
    }

    /// <summary>
    /// Converts the string to an int. If it cannot be converted, returns 0.
    /// </summary>
    public static int ToInt(this string str)
    {
        return str.ToNInt() ?? 0;
    }

    /// <summary>
    /// Converts the string to an int. If it cannot be converted, returns null.
    /// </summary>
    public static int? ToNInt(this string str)
    {
        if (int.TryParse(str, out int value))
        {
            return value;
        }
        return null;
    }

    /// <summary>
    /// Converts the string to a float. If it cannot be converted, returns 0.
    /// </summary>
    public static float ToFloat(this string str)
    {
        if (float.TryParse(str, out float value))
        {
            return value;
        }
        return 0;
    }

    /// <summary>
    /// Converts the string to a bool. True only if it is equal to "1".
    /// </summary>
    public static bool ToBool(this string str)
    {
        return str == "1";
    }

    /// <summary>
    /// Converts the string to a nullable boolean.
    /// </summary>
    /// <remarks>
    /// "1" is true, "0" is false, everything else is null.
    /// </remarks>
    public static bool? ToNBool(this string str)
    {
        return str switch
        {
            "0" => false,
            "1" => true,
            _ => null,
        };
    }

    /// <summary>
    /// Attempts to get a UInt from the dictionary at the given key. If the key does not exist or the value cannot be converted, returns null.
    /// </summary>
    public static uint? GetNUint(this Dictionary<string, string> dictionary, string key)
    {
        if (dictionary.TryGetValue(key, out var value))
        {
            return value.ToNUint();
        }
        return null;
    }

    /// <summary>
    /// Attempts to convert the value of each <see cref="keys"/> in <see cref="data"/> to a uint. If it cannot be converted, it will not be in the return list.
    /// </summary>
    public static List<uint> GetUints(this Dictionary<string, string> data, params string[] keys)
    {
        List<uint> result = new List<uint>();
        foreach (var key in keys)
        {
            var value = data.GetNUint(key);
            if (value != null)
            {
                result.Add(value.Value);
            }
        }
        return result;
    }

    /// <summary>
    /// Gets the maximum number of levels allowed pre-epic and maximum levels total for a class, using defaults in place of bad values.
    /// </summary>
    public static (uint maxPreEpicLevel, uint maxLevel) GetMaxLevels(this ClassModel classModel, uint defaultMaxPreEpic, uint defaultMax)
    {
        uint maxLevel;
        uint maxPreEpicLevel;
        if (classModel.MaxLevel == 0)
        { // Replace with default.
            maxLevel = defaultMax;
        }
        else
        { // "Valid" value in the class model
            maxLevel = classModel.MaxLevel;
        }
        if (classModel.EpicLevel < 0)
        { // Replace with default.
            maxPreEpicLevel = defaultMaxPreEpic;
        }
        else
        { // "Valid" value in the class model
            maxPreEpicLevel = (uint)classModel.EpicLevel;
        }

        // Some classes have a max level level as a limiting factor pre-epic.
        maxPreEpicLevel = Math.Min(maxLevel, maxPreEpicLevel);

        // Epic levels are allowed, but this number might be higher than allowed, such as prestige classes having 40 as their highest.
        maxLevel = Math.Min(maxLevel, defaultMax - defaultMaxPreEpic + maxPreEpicLevel);

        return (maxPreEpicLevel, maxLevel);
    }

    public static List<string> Split2daRow(this string rowString)
    {
        var values = new List<string>();
        for (int i = 0; i < rowString.Length; i++)
        {
            if (!char.IsWhiteSpace(rowString[i]))
            {
                if (rowString[i] == '"')
                { // Start of a column value that has whitespace. Use all characters until another quotation mark.
                    i++;
                    string value = "";
                    for (; i < rowString.Length; i++)
                    {
                        if (rowString[i] != '"')
                        {
                            value += rowString[i];
                        }
                        else
                        {
                            i++;
                            break;
                        }
                    }
                    values.Add(value);
                }
                else
                { // Start of a column value that does not contain whitespace. Use all characters until whitespace.
                    string value = "";
                    for (; i < rowString.Length; i++)
                    {
                        if (!char.IsWhiteSpace(rowString[i]))
                        {
                            value += rowString[i];
                        }
                        else
                        {
                            break;
                        }
                    }
                    values.Add(value);
                }
            }
        }
        return values;
    }

    /// <summary>
    /// Converts the input to the specified type if possible.
    /// </summary>
    public static T To<T>(this string input) where T : notnull
    {
        if (input == Constants.NullString)
        {
            return default!;
        }
        var converter = TypeDescriptor.GetConverter(typeof(T));
        var tResult = (T?)converter.ConvertFromString(input);
        if (tResult == null)
        {
            return default!;
        }
        return tResult;
    }

    /// <summary>
    /// Sets which alignments are allowed based on the restrictions.
    /// </summary>
    public static AlignmentsAllowed GetAlignmentsAllowed(AlignmentRestrictions restrictions, AlignmentRestrictionTypes neutralRestrictionTypes, bool invertRestrictions)
    {
        var result = new AlignmentsAllowed();

        result.LawfulGood = !(
            restrictions.HasFlag(AlignmentRestrictions.Lawful)
                || restrictions.HasFlag(AlignmentRestrictions.Good));
        result.NeutralGood = !(
            (restrictions.HasFlag(AlignmentRestrictions.Neutral)
                && neutralRestrictionTypes.HasFlag(AlignmentRestrictionTypes.GoodEvil))
            || restrictions.HasFlag(AlignmentRestrictions.Good));
        result.ChaoticGood = !(
            restrictions.HasFlag(AlignmentRestrictions.Chaotic)
                || restrictions.HasFlag(AlignmentRestrictions.Good));

        result.LawfulNeutral = !(
            (restrictions.HasFlag(AlignmentRestrictions.Neutral)
                && neutralRestrictionTypes.HasFlag(AlignmentRestrictionTypes.LawfulChaotic))
            || restrictions.HasFlag(AlignmentRestrictions.Lawful));
        result.TrueNeutral = !restrictions.HasFlag(AlignmentRestrictions.Neutral);
        result.ChaoticNeutral = !(
            (restrictions.HasFlag(AlignmentRestrictions.Neutral)
                && neutralRestrictionTypes.HasFlag(AlignmentRestrictionTypes.LawfulChaotic))
            || restrictions.HasFlag(AlignmentRestrictions.Chaotic));

        result.LawfulEvil = !(
            restrictions.HasFlag(AlignmentRestrictions.Lawful)
                || restrictions.HasFlag(AlignmentRestrictions.Evil));
        result.NeutralEvil = !(
            (restrictions.HasFlag(AlignmentRestrictions.Neutral)
                && neutralRestrictionTypes.HasFlag(AlignmentRestrictionTypes.GoodEvil))
            || restrictions.HasFlag(AlignmentRestrictions.Evil));
        result.ChaoticEvil = !(
            restrictions.HasFlag(AlignmentRestrictions.Chaotic)
                || restrictions.HasFlag(AlignmentRestrictions.Evil));

        if (invertRestrictions)
        {
            result.LawfulGood = !result.LawfulGood;
            result.NeutralGood = !result.NeutralGood;
            result.ChaoticGood = !result.ChaoticGood;

            result.LawfulNeutral = !result.LawfulNeutral;
            result.TrueNeutral = !result.TrueNeutral;
            result.ChaoticNeutral = !result.ChaoticNeutral;

            result.LawfulEvil = !result.LawfulEvil;
            result.NeutralEvil = !result.NeutralEvil;
            result.ChaoticEvil = !result.ChaoticEvil;
        }
        return result;
    }
}
