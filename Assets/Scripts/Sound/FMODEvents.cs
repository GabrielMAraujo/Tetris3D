using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class FMODEvents
{
    public enum SFX
    {
        [Description("event:/Rotation")]
        ROTATION
    }

    public enum GlobalParameters
    {
        [Description("Direction")]
        DIRECTION
    }

    //Get event string in Description data annotation
    public static string GetString<TEnum>(this TEnum enumValue) where TEnum : struct
    {
        return FMODEvents.GetEnumDescription((Enum)(object)((TEnum)enumValue));
    }

    private static string GetEnumDescription(Enum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());

        DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

        if (attributes != null && attributes.Any())
        {
            return attributes.First().Description;
        }

        return value.ToString();
    }
}
