using System;
using UnityEngine;

public class StringConvert
{
    public static object ToValue(Type type, string stringValue)
    {
        object obj = null;
        var sValue = ClearEndEmpty(stringValue);

        if (type.IsEnum)
        {
            obj = Enum.Parse(type, sValue);
        }
        else if (typeof(string).Equals(type))
        {
            obj = sValue;
        }
        else if (typeof(bool).Equals(type))
        {
            obj = sValue.Equals("1");
        }
        else if (typeof(int).Equals(type))
        {
            int.TryParse(sValue, out int value);
            obj = value;
        }
        else if (typeof(float).Equals(type))
        {
            float.TryParse(sValue, out float value);
            obj = value;
        }
        else if (typeof(Sprite).Equals(type))
        {
            var sprite = sValue.Split('_');
            obj = Resources.LoadAll<Sprite>(sprite[0])[int.Parse(sprite[1])];
        }

        return obj;
    }

    public static string ClearEndEmpty(string s)
    {
        return s.TrimEnd(new char[] { ' ', '\r', '\n' });
    }
}
