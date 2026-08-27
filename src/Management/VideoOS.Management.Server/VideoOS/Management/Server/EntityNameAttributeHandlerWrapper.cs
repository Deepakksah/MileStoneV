using System;
using System.Reflection;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public static class EntityNameAttributeHandlerWrapper
{
	private static volatile EntityNameAttributeHandler classAttributeHandler;

	private static readonly object locker = new object();

	public static void EnsureExecutingAssemblyScanned()
	{
		if (classAttributeHandler != null)
		{
			return;
		}
		lock (locker)
		{
			if (classAttributeHandler == null)
			{
				Assembly executingAssembly = Assembly.GetExecutingAssembly();
				EntityNameAttributeHandler entityNameAttributeHandler = new EntityNameAttributeHandler();
				entityNameAttributeHandler.ScanAssembly(executingAssembly);
				classAttributeHandler = entityNameAttributeHandler;
			}
		}
	}

	public static string GetEntityName(Type classType)
	{
		EnsureExecutingAssemblyScanned();
		return classAttributeHandler.GetEntityName(classType);
	}

	public static Type GetEntityClassType(string entityName)
	{
		EnsureExecutingAssemblyScanned();
		return classAttributeHandler.GetEntityClassType(entityName);
	}

	public static Type GetEntityClassTypeNoAssert(string entityName)
	{
		EnsureExecutingAssemblyScanned();
		return classAttributeHandler.GetEntityClassType(entityName, assert: false);
	}

	public static Type GetFunctionClassType(string functionName)
	{
		EnsureExecutingAssemblyScanned();
		return classAttributeHandler.GetFunctionClassType(functionName);
	}
}
