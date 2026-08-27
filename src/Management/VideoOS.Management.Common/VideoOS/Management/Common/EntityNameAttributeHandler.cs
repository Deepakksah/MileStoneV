using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VideoOS.Management.Common;

public class EntityNameAttributeHandler
{
	private class NameAndClassEntry
	{
		public Type ClassType { get; private set; }

		public string Name { get; private set; }

		public NameAndClassEntry(Type classType, string entityName)
		{
			ClassType = classType;
			Name = entityName;
		}
	}

	private List<NameAndClassEntry> entityClassList;

	private List<NameAndClassEntry> functionClassList;

	public void ScanAssembly(Assembly assembly)
	{
		if (assembly == null)
		{
			throw new ArgumentNullException("assembly");
		}
		if (entityClassList != null)
		{
			return;
		}
		entityClassList = new List<NameAndClassEntry>();
		functionClassList = new List<NameAndClassEntry>();
		Type[] types = assembly.GetTypes();
		foreach (Type type in types)
		{
			object[] customAttributes = type.GetCustomAttributes(typeof(EntityNameAttribute), inherit: false);
			for (int j = 0; j < customAttributes.Length; j++)
			{
				EntityNameAttribute entityNameAttribute = (EntityNameAttribute)customAttributes[j];
				entityClassList.Add(new NameAndClassEntry(type, entityNameAttribute.Name));
			}
			customAttributes = type.GetCustomAttributes(typeof(FunctionHandlerAttribute), inherit: false);
			for (int j = 0; j < customAttributes.Length; j++)
			{
				FunctionHandlerAttribute functionHandlerAttribute = (FunctionHandlerAttribute)customAttributes[j];
				functionClassList.Add(new NameAndClassEntry(type, functionHandlerAttribute.Name));
			}
		}
	}

	public string GetEntityName(Type classType)
	{
		string result = null;
		NameAndClassEntry nameAndClassEntry = entityClassList.FirstOrDefault((NameAndClassEntry enct) => enct.ClassType == classType);
		if (nameAndClassEntry != null)
		{
			result = nameAndClassEntry.Name;
		}
		return result;
	}

	public Type GetEntityClassType(string entityName)
	{
		return GetEntityClassType(entityName, assert: true);
	}

	public Type GetEntityClassType(string entityName, bool assert)
	{
		Type result = null;
		NameAndClassEntry nameAndClassEntry = entityClassList.FirstOrDefault((NameAndClassEntry enct) => enct.Name == entityName);
		if (nameAndClassEntry != null)
		{
			result = nameAndClassEntry.ClassType;
		}
		return result;
	}

	public Type GetFunctionClassType(string functionName)
	{
		Type result = null;
		NameAndClassEntry nameAndClassEntry = functionClassList.FirstOrDefault((NameAndClassEntry e) => e.Name == functionName);
		if (nameAndClassEntry != null)
		{
			result = nameAndClassEntry.ClassType;
		}
		return result;
	}
}
