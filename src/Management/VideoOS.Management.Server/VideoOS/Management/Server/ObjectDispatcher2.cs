using System;
using System.Reflection;

namespace VideoOS.Management.Server;

public static class ObjectDispatcher2
{
	public static IGenericObjectSupport2 GetObjectByEntityName(string entityName)
	{
		Type entityClassType = EntityNameAttributeHandlerWrapper.GetEntityClassType(entityName);
		if (entityClassType == null)
		{
			throw new NotSupportedException("No generic business object registered on server with entity name: " + entityName);
		}
		object[] args = new object[0];
		return (IGenericObjectSupport2)Activator.CreateInstance(entityClassType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, args, null);
	}

	public static IGenericFunction GetObjectByFunctionName(string functionName)
	{
		Type functionClassType = EntityNameAttributeHandlerWrapper.GetFunctionClassType(functionName);
		if (functionClassType == null)
		{
			throw VmoExceptionFactory.VMO60311_VmoFunctionNotSupportedException(functionName ?? "<null>");
		}
		object[] args = new object[0];
		return (IGenericFunction)Activator.CreateInstance(functionClassType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, args, null);
	}
}
