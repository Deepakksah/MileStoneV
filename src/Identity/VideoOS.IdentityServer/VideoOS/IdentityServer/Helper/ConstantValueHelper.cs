using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VideoOS.IdentityServer.Helper;

public class ConstantValueHelper
{
	public static List<T> GetConstantValues<T>(Type type)
	{
		FieldInfo[] fields = type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
		return (from fi in fields
			where fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(T)
			select (T)fi.GetRawConstantValue()).ToList();
	}
}
