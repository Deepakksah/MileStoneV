using System;
using System.Collections.Generic;
using System.Reflection;

namespace VideoOS.LPR.Common.OEM;

public static class MissingResourcesFinder
{
	public static IEnumerable<string> LookupMissingProperties(Type resourceToBeTested)
	{
		List<string> list = new List<string>();
		if (resourceToBeTested != null)
		{
			BindingFlags bindingAttr = BindingFlags.Static | BindingFlags.Public;
			PropertyInfo[] properties = resourceToBeTested.GetProperties(bindingAttr);
			foreach (PropertyInfo obj in properties)
			{
				string name = obj.Name;
				if (obj.GetValue(null, null) is string text && text == "[" + name + "]")
				{
					list.Add(resourceToBeTested.Name + "." + name);
				}
			}
		}
		return list;
	}
}
