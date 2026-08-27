using System;
using System.ComponentModel;
using System.Reflection;

namespace VideoOS.Server.CommandHandler;

public static class Extensions
{
	public static string ToDescription(this Enum value)
	{
		FieldInfo field = value.GetType().GetField(value.ToString());
		if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute descriptionAttribute)
		{
			return descriptionAttribute.Description;
		}
		return value.ToString();
	}
}
