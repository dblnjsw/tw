using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CsFunction
{
    public static bool IsNumeric(string value)
    {
        return Regex.IsMatch(value, @"^[+-]?/d*[.]?/d*$");
    }
}
