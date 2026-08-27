using System;
using System.Reflection;

namespace VideoOS.Mobile.UI.ViewModel.Extensions;

public static class TypeExtentions
{
	public static PropertyInfo[] GetAllProperties(this Type type)
	{
		return type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
	}

	public static bool IsFromType(this Type type, Type otherType)
	{
		if (!(type == otherType) && !type.IsSubclassOf(otherType))
		{
			return otherType.IsAssignableFrom(type);
		}
		return true;
	}
}
