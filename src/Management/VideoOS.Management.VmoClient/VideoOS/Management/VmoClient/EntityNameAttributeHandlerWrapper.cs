using System;
using System.Reflection;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

public static class EntityNameAttributeHandlerWrapper
{
	private static volatile EntityNameAttributeHandler entityNameAttributeHandler;

	private static readonly object locker = new object();

	public static void ScanAssemblyForClassAttributeEntityName()
	{
		if (entityNameAttributeHandler != null)
		{
			return;
		}
		lock (locker)
		{
			if (entityNameAttributeHandler == null)
			{
				Assembly executingAssembly = Assembly.GetExecutingAssembly();
				EntityNameAttributeHandler obj = new EntityNameAttributeHandler();
				obj.ScanAssembly(executingAssembly);
				entityNameAttributeHandler = obj;
			}
		}
	}

	public static string GetEntityName(Type classType)
	{
		if (classType == null)
		{
			return null;
		}
		ScanAssemblyForClassAttributeEntityName();
		return entityNameAttributeHandler.GetEntityName(classType);
	}

	public static Type GetClassType(string entityName)
	{
		if (entityName == null)
		{
			return null;
		}
		ScanAssemblyForClassAttributeEntityName();
		return entityNameAttributeHandler.GetEntityClassType(entityName);
	}
}
